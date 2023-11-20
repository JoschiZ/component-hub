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
    void Add(TEntity entity);
    ValueTask AddAsync(TEntity entity);
    ValueTask AddAsync(TEntity entity, CancellationToken ct);
    void AddRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}