using ComponentHub.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ComponentHub.DB.Core;

internal class Repository<TItem, TKey> : IRepository<TItem, TKey>
    where TItem : class, IAggregateRoot<TKey>
    where TKey: struct, IEquatable<TKey>
{
    protected readonly ComponentHubContext Context;
    protected readonly DbSet<TItem> Set;

    public Repository(ComponentHubContext context)
    {
        Context = context;
        Set = Context.Set<TItem>();
    }

    public void Attach(TItem item)
    {
        Context.Attach(item);
    }

    public ValueTask<EntityEntry<TItem>> AddAsync(TItem item, CancellationToken ct)
    {
        return Set.AddAsync(item, ct);
    }

    public Task<int> RemoveByIdAsync(TKey id, CancellationToken ct)
    {
        return Set.Where(item => item.Id.Equals(id)).ExecuteDeleteAsync(cancellationToken: ct);
    }
}