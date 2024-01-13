using ComponentHub.DB;
using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Core.Primitives.Results;
using ComponentHub.Domain.Features.Components;
using ComponentHub.Domain.Features.Users;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace ComponentHub.Server.Features.Components.UpdateComponent;

internal sealed class
    UpdateComponentEndpoint : Endpoint<UpdateComponentRequest, Results<Ok, UnauthorizedHttpResult, ProblemDetails, BadRequest<string>>>
{

    private readonly IDbContextFactory<ComponentHubContext> _contextFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public UpdateComponentEndpoint(IDbContextFactory<ComponentHubContext> contextFactory, UserManager<ApplicationUser> userManager)
    {
        _contextFactory = contextFactory;
        _userManager = userManager;
    }

    public override void Configure()
    {
        Patch(Endpoints.Components.Update);       
    }


    public override async Task<Results<Ok, UnauthorizedHttpResult, ProblemDetails, BadRequest<string>>> ExecuteAsync(UpdateComponentRequest req, CancellationToken ct)
    {
        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return TypedResults.Unauthorized();
        }

        await using var context = await _contextFactory.CreateDbContextAsync(ct);
        var updateResult = await UpdateComponent(
                new UserId(Guid.Parse(userId)),
                req.PageId, 
                req.ComponentId,
                req.Name, 
                req.SourceCode, 
                req.Height, 
                req.Width, 
                req.WclComponentId,
                context, 
                ct: ct);

        if (updateResult.IsT0)
        {
            await context.SaveChangesAsync(ct);
        }
        return updateResult.Match<Results<Ok, UnauthorizedHttpResult, ProblemDetails, BadRequest<string>>>(
                component => TypedResults.Ok(),
                error => TypedResults.BadRequest(error.ErrorCode),
                list => new ProblemDetails(list));
        
    }


    private static async Task<OneOf<ComponentPage, Error, List<ValidationFailure>>> UpdateComponent(
        UserId userId,
        ComponentPageId reqPageId,
        ComponentId reqComponentId,
        string reqName,
        string reqSourceCode,
        short reqHeight,
        short reqWidth,
        Guid reqWclComponentId,
        ComponentHubContext context,
        CancellationToken ct)
    {
        var root = await context.Components
            .Where(entry => entry.Owner.Id == userId)
            .FirstOrDefaultAsync(entry => entry.Id == reqPageId, cancellationToken: ct);

        if (root is null)
        {
            return Error.UserNotFoundError;
        }

        var currentComponent = root.GetCurrentComponent();

        var updatedSourceResult = currentComponent.Source.TryGetUpdatedSource(reqSourceCode, reqHeight, reqWidth, reqWclComponentId);

        if (updatedSourceResult.IsError)
        {
            return updatedSourceResult.Error;
        }

        var updatedSource = updatedSourceResult.ResultObject;

        var updatedComponentResult = Component.TryCreate(updatedSource, root.Id, root);

        if (updatedComponentResult.IsError)
        {
            return updatedComponentResult.Error;
        }

        var updatedComponent = updatedComponentResult.ResultObject;

        var updateResult = root.UpdateCurrentComponent(updatedComponent);

        return updateResult.IsSuccess ? updateResult.ResultObject : updateResult.Error;
    }
}
