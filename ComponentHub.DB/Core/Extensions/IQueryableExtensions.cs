using System.Diagnostics.Contracts;
using System.Security.Claims;
using ComponentHub.Domain.Core.Interfaces;
using ComponentHub.Domain.Features.Users;

namespace ComponentHub.DB.Core.Extensions;

public static class QueryableExtensions
{
    [Pure]
    public static IQueryable<T> IsCurrentUser<T>(this IQueryable<T> query, ClaimsPrincipal principal) where T : IHasOwner
    {
        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (id is null)
        {
            return query.Where(owner => false);
        }

        var userId = new UserId(new Guid(id));

        return query.Where(owner => owner.Owner.Id == userId);
    }
}