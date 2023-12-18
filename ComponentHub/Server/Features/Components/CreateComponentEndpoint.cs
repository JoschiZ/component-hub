using ComponentHub.DB.Core;
using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Components;
using ComponentHub.Domain.Features.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Components;

internal sealed class CreateComponentEndpoint: Endpoint<CreateComponentRequest, Results<Ok, ProblemDetails, UnauthorizedHttpResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<CreateComponentEndpoint> _logger;
    private readonly IUnitOfWorkFactory _workFactory;

    public CreateComponentEndpoint(UserManager<ApplicationUser> userManager, ILogger<CreateComponentEndpoint> logger, IUnitOfWorkFactory workFactory)
    {
        _userManager = userManager;
        _logger = logger;
        _workFactory = workFactory;
    }

    public override void Configure()
    {
        Put(Endpoints.Components.Create);
    }

    public override async Task<Results<Ok, ProblemDetails, UnauthorizedHttpResult>> ExecuteAsync(CreateComponentRequest req, CancellationToken ct)
    {
        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return TypedResults.Unauthorized();
        }

        var unitOfWork = _workFactory.GetUnitOfWork();
        //var user = await _userManager.FindByIdAsync(userId);

        var user = await unitOfWork.UserSet.FindAsync(new object?[] { new UserId(new Guid(userId)) }, cancellationToken: ct);
        
        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        var component = Component.TryCreate(req.SourceCode, user, req.Name);

        if (component.IsError)
        {
            ValidationFailures.AddRange(component.Error);
            return new ProblemDetails(ValidationFailures);
        }

        await unitOfWork.Components.AddAsync(component.ResultObject, ct);
        await unitOfWork.CompletedAsync(ct);
        return TypedResults.Ok();
    }
}