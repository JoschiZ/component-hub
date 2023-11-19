using ComponentHub.Shared.DatabaseObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ComponentHub.Shared.Helper.Repositories;

internal abstract class Repository<TEntity, TId>(DbContext context) : IRepository<TEntity, TId>
    where TId : struct
    where TEntity : Entity<TId>
{
    private protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public TEntity? Get(TId id)
    {
        return _dbSet.Find(id); 
    }

    public ValueTask<TEntity?> GetAsync(TId id)
    {
        return GetAsync(id, CancellationToken.None);
    }

    public ValueTask<TEntity?> GetAsync(TId id, CancellationToken ct)
    {
        return _dbSet.FindAsync(new object?[] {id}, cancellationToken: ct);
    } 

    public EntityEntry<TEntity> Add(TEntity entity)
    {
        return _dbSet.Add(entity);
    }

    public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity)
    {
        return AddAsync(entity, CancellationToken.None);
    }

    public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken ct)
    {
        return _dbSet.AddAsync(entity, ct);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}