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
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateComponentEndpoint> _logger;

    public CreateComponentEndpoint(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, ILogger<CreateComponentEndpoint> logger)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
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
        var user = await _userManager.FindByIdAsync(userId);

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

        await _unitOfWork.Components.AddAsync(component.ResultObject, ct);
        await _unitOfWork.CompletedAsync(ct);
        return TypedResults.Ok();
    }
}