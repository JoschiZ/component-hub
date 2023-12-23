using ComponentHub.Domain.Core;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ComponentHub.DB.Core;

public interface IRepository<TItem, TId> 
    where TItem : class, IAggregateRoot<TId>
    where TId: struct, IEquatable<TId>
{
    public void Attach(TItem item);
    public ValueTask<EntityEntry<TItem>> AddAsync(TItem item, CancellationToken ct);
    public Task<int> RemoveByIdAsync(TId id, CancellationToken ct);
}