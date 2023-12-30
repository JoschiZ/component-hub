using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using ComponentHub.Server.Features.Components;

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
}