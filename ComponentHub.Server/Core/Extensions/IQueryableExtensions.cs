using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Security.Claims;
using ComponentHub.Domain.Core.Interfaces;
using ComponentHub.Domain.Features.Users;
using ComponentHub.Server.Features.Components.QueryEndpoints;

namespace ComponentHub.Server.Core.Extensions;

internal static class QueryableExtensions
{
    [Pure]
    public static IOrderedQueryable<T> OrderByWithDirection<T, TKey>(
        this IQueryable<T> query,
        SortDirection direction, 
        Expression<Func<T, TKey>> selector)
    {
        return direction switch
        {
            SortDirection.Ascending => query.OrderBy(selector),
            SortDirection.Descending => query.OrderByDescending(selector),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    [Pure]
    public static IOrderedQueryable<T> ThenByWithDirection<T, TKey>(
        this IOrderedQueryable<T> query,
        SortDirection direction, 
        Expression<Func<T, TKey>> selector)
    {
        return direction switch
        {
            SortDirection.Ascending => query.ThenBy(selector),
            SortDirection.Descending => query.ThenByDescending(selector),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

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
    
    
    
    [Pure]
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, IPaginatedRequest pager)
    {
        return query.Skip(pager.Page * pager.PageSize).Take(pager.PageSize);
    }
}