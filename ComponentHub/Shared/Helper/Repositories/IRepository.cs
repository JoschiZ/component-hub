using ComponentHub.Shared.DatabaseObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ComponentHub.Shared.Helper.Repositories;

public interface IRepository<TEntity, in TId> 
    where TId: struct 
    where TEntity: Entity<TId>
{
    TEntity? Get(TId id);
    public ValueTask<TEntity?> GetAsync(TId id);
    public ValueTask<TEntity?> GetAsync(TId id, CancellationToken ct);
    EntityEntry<TEntity> Add(TEntity entity);
    ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity);
    ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken ct);
    void AddRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}