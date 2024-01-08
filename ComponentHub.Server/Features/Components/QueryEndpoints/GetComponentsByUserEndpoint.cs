using ComponentHub.DB;
using ComponentHub.Domain.Constants;
using ComponentHub.Server.Core;
using ComponentHub.Server.Core.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Server.Features.Components.QueryEndpoints;

internal sealed class
    GetComponentsByUserEndpoint : Endpoint<GetComponentsByUserEndpoint.Request, Ok<GetComponentsByUserEndpoint.ResponseDto>>
{
    private readonly IDbContextFactory<ComponentHubContext> _contextFactory;

    public GetComponentsByUserEndpoint(IDbContextFactory<ComponentHubContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public override void Configure()
    {
        Get(Endpoints.Components.GetByUser);
        AllowAnonymous();
    }

    public override async Task<Ok<ResponseDto>> ExecuteAsync(Request req, CancellationToken ct)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(ct);

        var query = context.Components
            .Include(entry => entry.Owner)
            .Where(entry => entry.Owner.UserName == req.UserName)
            .OrderBy(entry => entry.Name)
            .Paginate(req)
            .Select(entry => entry.ToDto());

        query.TryGetNonEnumeratedCount(out var overallCount);

        var data = await query.ToArrayAsync(cancellationToken: ct);

        var response = new ResponseDto(
            data,
            ResponsePagination.CreateFromRequest(req, overallCount));
        return TypedResults.Ok(response);
    }
    
    internal sealed record Request(string UserName, int Page = 0, int PageSize = 10) : IPaginatedRequest
    {

    }

    internal sealed record ResponseDto(ComponentEntryDto[] Components, ResponsePagination Pagination): IPaginatedResponse;
}

