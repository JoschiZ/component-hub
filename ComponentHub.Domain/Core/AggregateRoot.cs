namespace ComponentHub.Domain.Core;

public abstract class AggregateRoot<TId>: Entity<TId>,  IAggregateRoot<TId> where TId: struct, IEquatable<TId>
{
    [Obsolete("EFCOre Constructor")]
    protected AggregateRoot(): base(){}
    
    protected AggregateRoot(TId id) : base(id) { }
}

public interface IAggregateRoot<out TId>
{
    public TId Id { get; }
}