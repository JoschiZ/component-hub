using ComponentHub.DB.Core;
using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Components;
using ComponentHub.Server.Core.ResponseObjects;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ComponentHub.Server.Features.Components;

internal sealed class DeleteComponentEndpoint : Endpoint<DeleteComponentRequest, Results<Ok, NotFound<Error404>>>
{
    public DeleteComponentEndpoint(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    
    public override void Configure()
    {
        Delete(Endpoints.Components.Delete);
    }

    public override async Task<Results<Ok, NotFound<Error404>>> ExecuteAsync(DeleteComponentRequest req, CancellationToken ct)
    {
        await using var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();
        var deletedRows = await unitOfWork.Components.RemoveByIdAsync(req.ComponentId, ct);
        if (deletedRows < 1)
        {
            return TypedResults.NotFound(new Error404());
        }
        await unitOfWork.CompletedAsync(ct);
        return TypedResults.Ok();
    }
}

public record struct DeleteComponentRequest(ComponentEntryId ComponentId);