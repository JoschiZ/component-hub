using ComponentHub.DB;
using ComponentHub.Domain.Constants;
using ComponentHub.Server.Core.ResponseObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Server.Features.Components.QueryEndpoints;

internal class GetComponentEndpoint: Endpoint<GetComponentRequest, Results<Ok<GetComponentResponse>, NotFound<Error404>, ProblemHttpResult>>
{
    private readonly IDbContextFactory<ComponentHubContext> _contextFactory;

    public GetComponentEndpoint(IDbContextFactory<ComponentHubContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public override void Configure()
    {
        Get(Endpoints.Components.Get);
        AllowAnonymous();
    }

    public override async Task<Results<Ok<GetComponentResponse>, NotFound<Error404>, ProblemHttpResult>> ExecuteAsync(GetComponentRequest req, CancellationToken ct)
    {
        Console.WriteLine(req);
        await using var context = await _contextFactory.CreateDbContextAsync(ct);
        var entry = await context
            .Components
            .AsNoTracking()
            .Include(entry => entry.ComponentHistory)
            .Include(entry => entry.Owner)
            .Include(entry => entry.Tags)
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

internal sealed record GetComponentResponse(ComponentPageDto ComponentPage, ComponentDto CurrentComponent);
