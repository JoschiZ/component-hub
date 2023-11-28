namespace ComponentHub.Domain.Core;

public abstract class Entity<TId> where TId: struct
{
    protected Entity(){}

    protected Entity(TId id)
    {
        Id = id;
    }
    
    public TId Id { get; private init; }
}