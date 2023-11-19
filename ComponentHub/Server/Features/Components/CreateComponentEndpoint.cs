using ComponentHub.Shared.Api;
using ComponentHub.Shared.Auth;
using ComponentHub.Shared.Components;
using ComponentHub.Shared.DatabaseObjects;
using ComponentHub.Shared.Features.Components;
using ComponentHub.Shared.Helper.Validation;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Components;

internal sealed class CreateComponentEndpoint: Endpoint<CreateComponentRequest>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ComponentHubContext _context;


    public CreateComponentEndpoint(UserManager<ApplicationUser> userManager, ComponentHubContext context)
    {
        _userManager = userManager;
        _context = context;
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
        
        await _context.Set<Component>().AddAsync(component.ResultObject, ct);
    }
}

public class CreateComponentRequest
{
    public string Name { get; set; }
    public string SourceCode { get; set; }
    public Language Language { get; set; }
}

public class CreateComponentRequestValidator : MudCompatibleAbstractValidator<CreateComponentRequest>
{
    public CreateComponentRequestValidator()
    {
        RuleFor(request => request.Name).MaximumLength(ComponentSource.MaxSourceLength);
    }
}
