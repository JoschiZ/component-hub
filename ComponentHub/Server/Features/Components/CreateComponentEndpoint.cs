using ComponentHub.DB.BaseClasses;
using ComponentHub.DB.Core;
using ComponentHub.DB.Features.Components;
using ComponentHub.DB.Features.User;
using ComponentHub.Domain.Features.Components.CreateComponent;
using ComponentHub.Shared.Api;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Components;

internal sealed class CreateComponentEndpoint: Endpoint<CreateComponentRequest, Results<Ok, ProblemDetails, ProblemHttpResult>>
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

    public override async Task<Results<Ok, ProblemDetails, ProblemHttpResult>> ExecuteAsync(CreateComponentRequest req, CancellationToken ct)
    {
        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return TypedResults.Problem();
        }
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return TypedResults.Problem();
        }

        var component = Component.TryCreate(req.SourceCode, req.Language, user, req.Name);

        if (component.IsError)
        {
            ValidationFailures.AddRange(component.Error);
            return new ProblemDetails(ValidationFailures);
        }

        await _unitOfWork.Components.AddAsync(component.ResultObject, ct);
        return TypedResults.Ok();
    }
}