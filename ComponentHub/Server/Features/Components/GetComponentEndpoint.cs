using ComponentHub.DB.Core;
using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Server.Features.Components;

internal class GetComponentEndpoint: Endpoint<GetComponentRequest, Results<Ok<GetComponentResponse>, NotFound, ProblemHttpResult>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetComponentEndpoint(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public override void Configure()
    {
        Get(Endpoints.Components.Get + "{UserName}" + "/" + "{ComponentName}");
    }

    public override async Task<Results<Ok<GetComponentResponse>, NotFound, ProblemHttpResult>> ExecuteAsync(GetComponentRequest req, CancellationToken ct)
    {
        var unit = _unitOfWorkFactory.GetUnitOfWork();
        var entry = await unit.Components
            .QueryComponents()
            .Include(entry => entry.ComponentHistory)
            .Include(entry => entry.Owner)
            .FirstOrDefaultAsync(
                component => 
                    component.Name == req.ComponentName &&
                    component.Owner.UserName == req.UserName, ct);


        if (entry is not null)
        {
            return TypedResults.Ok(new GetComponentResponse(
                entry.ToDto(),
                entry.GetCurrentComponent().ToDto()));
        }

        return TypedResults.NotFound();
    }
}

internal readonly record struct GetComponentResponse(ComponentEntryDto ComponentEntry, ComponentDto CurrentComponent);