using ComponentHub.Shared.Api;
using ComponentHub.Shared.DatabaseObjects;
using ComponentHub.Shared.Features.Components;
using ComponentHub.Shared.Helper.Repositories;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Components;

internal sealed class CreateComponentEndpoint: Endpoint<CreateComponentRequest>
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

    public override async Task HandleAsync(CreateComponentRequest req, CancellationToken ct)
    {
        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            await SendForbiddenAsync(ct);
            return;
        }

        var component = Component.TryCreate(new ComponentId(Guid.NewGuid()), req.SourceCode, req.Language, user, req.Name);

        if (component.IsError)
        {
            ValidationFailures.AddRange(component.Error);
            ThrowIfAnyErrors();
            return;
        }

        await _unitOfWork.Components.AddAsync(component.ResultObject, ct);
    }
}