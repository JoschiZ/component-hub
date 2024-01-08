using ComponentHub.DB;
using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Features.Components;
using ComponentHub.Server.Core.ResponseObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Server.Features.Components;

internal sealed class DeleteComponentEndpoint : Endpoint<DeleteComponentRequest, Results<Ok, NotFound<Error404>>>
{
    public DeleteComponentEndpoint(IDbContextFactory<ComponentHubContext> unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    private readonly IDbContextFactory<ComponentHubContext> _unitOfWorkFactory;
    
    public override void Configure()
    {
        Delete(Endpoints.Components.Delete);
    }

    public override async Task<Results<Ok, NotFound<Error404>>> ExecuteAsync(DeleteComponentRequest req, CancellationToken ct)
    {
        await using var context = await _unitOfWorkFactory.CreateDbContextAsync(ct);
        var deletedRows = await context.Components
            .AsQueryable()
            .Where(entry => entry.Id == req.ComponentId)
            .ExecuteDeleteAsync(cancellationToken: ct);
        if (deletedRows < 1)
        {
            return TypedResults.NotFound(new Error404());
        }
        await context.SaveChangesAsync(ct);
        return TypedResults.Ok();
    }
}

public record DeleteComponentRequest(ComponentEntryId ComponentId);