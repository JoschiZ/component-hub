using ComponentHub.DB.Core;
using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Features.Users;
using ComponentHub.Server.Features.Components.UpdateComponent;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Components;

internal sealed class
    UpdateComponentEndpoint : Endpoint<UpdateComponentRequest, Results<Ok, UnauthorizedHttpResult, ProblemDetails, BadRequest<string>>>
{

    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public UpdateComponentEndpoint(IUnitOfWorkFactory unitOfWorkFactory, UserManager<ApplicationUser> userManager)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
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

        await using var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();
        var updateResult = await unitOfWork
            .Components
            .UpdateComponent(
                new UserId(Guid.Parse(userId)),
                req.EntryId, 
                req.ComponentId,
                req.Name, 
                req.SourceCode, 
                req.Height, 
                req.Width, 
                req.WclComponentId,
                ct: ct);

        if (updateResult.IsT0)
        {
            await unitOfWork.CompletedAsync(ct);
        }
        return updateResult.Match<Results<Ok, UnauthorizedHttpResult, ProblemDetails, BadRequest<string>>>(
                component => TypedResults.Ok(),
                error => TypedResults.BadRequest(error.ErrorCode),
                list => new ProblemDetails(list));
        
    }
}
