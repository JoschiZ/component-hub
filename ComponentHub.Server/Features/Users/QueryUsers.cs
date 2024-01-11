using ComponentHub.DB;
using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Features.Users;
using ComponentHub.Server.Core;
using ComponentHub.Server.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using PublicUserDto = ComponentHub.Domain.Features.Users.PublicUserDto;
using ResponsePagination = ComponentHub.Server.Core.ResponsePagination;

namespace ComponentHub.Server.Features.Users;

internal sealed class SearchUsers : Endpoint<SearchUsers.Request, SearchUsers.ResponseDto>
{
    private readonly IDbContextFactory<ComponentHubContext> _contextFactory;

    public SearchUsers(IDbContextFactory<ComponentHubContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public override void Configure()
    {
        Get(Endpoints.Users.Search);
        AllowAnonymous();
    }

    public override async Task<ResponseDto> ExecuteAsync(Request req, CancellationToken ct)
    {
        var context = await _contextFactory.CreateDbContextAsync(ct);

        if (req.UserName is null)
        {
            return new ResponseDto([], ResponsePagination.CreateFromRequest(req, 0));
        }

        var query = context.Users
            .AsNoTracking()
            .Where(user => user.UserName != null && user.UserName.Contains(req.UserName))
            .ProjectToDto();

        var totalItems = await query.CountAsync(ct);

        var results = await query.Paginate(req).ToArrayAsync(ct);

        return new ResponseDto(results, ResponsePagination.CreateFromRequest(req, totalItems));
    }

    internal sealed record Request(string? UserName, int Page = 0, int PageSize = 10): IPaginatedRequest
    {
    }

    internal sealed record ResponseDto(PublicUserDto[] PublicUserInfos, ResponsePagination Pagination): IPaginatedResponse
    {
    }
}