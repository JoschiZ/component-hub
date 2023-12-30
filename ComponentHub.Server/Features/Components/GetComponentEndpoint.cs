using ComponentHub.DB.Core;
using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Components;
using ComponentHub.Server.Core.ResponseObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Server.Features.Components;

internal class GetComponentEndpoint: Endpoint<GetComponentRequest, Results<Ok<GetComponentResponse>, NotFound<Error404>, ProblemHttpResult>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetComponentEndpoint(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public override void Configure()
    {
        Get(Endpoints.Components.Get);
        AllowAnonymous();
    }

    public override async Task<Results<Ok<GetComponentResponse>, NotFound<Error404>, ProblemHttpResult>> ExecuteAsync(GetComponentRequest req, CancellationToken ct)
    {
        Console.WriteLine(req);
        var unit = _unitOfWorkFactory.GetUnitOfWork();
        var entry = await unit.Components
            .Query()
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

        return TypedResults.NotFound(new Error404());
    }
}


internal sealed record GetComponentRequest(string UserName, string ComponentName);

internal sealed record GetComponentResponse(ComponentEntryDto ComponentEntry, ComponentDto CurrentComponent);
