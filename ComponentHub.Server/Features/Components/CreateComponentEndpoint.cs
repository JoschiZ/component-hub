using ComponentHub.DB.Core;
using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Features.Components;
using ComponentHub.Domain.Features.Users;
using ComponentHub.Server.Features.Components.CreateComponent;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Server.Features.Components;

internal sealed class CreateComponentEndpoint: Endpoint<CreateComponentRequest, Results<Created<CreateComponentResponse>, ProblemDetails, UnauthorizedHttpResult, Conflict<string>>>
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

    public override async Task<Results<Created<CreateComponentResponse>, ProblemDetails, UnauthorizedHttpResult, Conflict<string>>> ExecuteAsync(CreateComponentRequest req, CancellationToken ct)
    {
        var userId = _userManager.GetUserId(User);
        var userName = _userManager.GetUserName(User);
        if (userId is null || userName is null)
        {
            return TypedResults.Unauthorized();
        }

        var unitOfWork = _workFactory.GetUnitOfWork();

        // Create a Stub user to attach the component to
        var user = new ApplicationUser() {Id = new UserId(new Guid(userId)), UserName = userName};

        var componentSource = ComponentSource.TryCreate(req.SourceCode, req.Height, req.Width, req.WclComponentId);
        if (componentSource.IsError)
        {
            return new ProblemDetails(componentSource.Error);
        }

        var entryId = ComponentEntryId.New();
        var componentResult = Component.TryCreate(componentSource.ResultObject, req.Name, entryId);

        if (componentResult.IsError)
        {
            return new ProblemDetails(componentResult.Error);
        }

        var component = componentResult.ResultObject;
        
        var componentEntry = ComponentEntry.TryCreate(entryId, req.Name, req.Description, componentResult.ResultObject, user);


        if (componentEntry.IsError)
        {
            ValidationFailures.AddRange(componentEntry.Error);
            return new ProblemDetails(ValidationFailures);
        }


        try
        {
            unitOfWork.Attach(user);
            await unitOfWork.Components.AddAsync(componentEntry.ResultObject, ct);
            await unitOfWork.CompletedAsync(ct);
            
            return TypedResults.Created(
                new Uri(BaseURL + Endpoints.Components.FormatGet(userName, component.Name)), 
                new CreateComponentResponse(componentEntry.ResultObject.ToDto(), componentResult.ResultObject.ToDto()));
        }
        catch (DbUpdateException e)
        {
            return TypedResults.Conflict("A component with this name already exists");
        }
    }
}

internal record CreateComponentResponse(ComponentEntryDto Entry, ComponentDto Component);