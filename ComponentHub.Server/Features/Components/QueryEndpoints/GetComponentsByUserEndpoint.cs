using ComponentHub.DB.Core;
using ComponentHub.Domain.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Server.Features.Components.QueryEndpoints;

internal sealed class
    GetComponentsByUserEndpoint : Endpoint<GetComponentsByUserRequest, Ok<ComponentEntryDto[]>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetComponentsByUserEndpoint(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public override void Configure()
    {
        Get(Endpoints.Components.GetByUser);
        AllowAnonymous();
    }

    public override async Task<Ok<ComponentEntryDto[]>> ExecuteAsync(GetComponentsByUserRequest req, CancellationToken ct)
    {
        await using var uof = _unitOfWorkFactory.GetUnitOfWork();
        
        var found = await uof.Components
            .Query()
            .Where(entry => entry.Owner.UserName == req.UserName)
            .OrderBy(entry => entry.Name)
            .Skip(req.Page * req.PageSize)
            .Take(req.PageSize)
            .Select(entry => entry.ToDto())
            .ToArrayAsync(ct);

        return TypedResults.Ok(found);
    }
}

internal sealed record GetComponentsByUserRequest(string UserName, int Page = 0, int PageSize = 20);

internal sealed record GetComponentsByUserResponse(ComponentEntryDto[] Components);