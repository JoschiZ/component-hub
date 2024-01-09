using ComponentHub.Domain.Core;
using ComponentHub.Domain.Core.Interfaces;
using ComponentHub.Domain.Features.Users;
using StronglyTypedIds;

namespace ComponentHub.Domain.Features.Components;

internal sealed partial class TestNewModeling
{
    internal sealed class ComponentPage: AggregateRoot<ComponentPageId>, IHasOwner
    {
        public List<Comment> Comments { get; private set; } = [];
        
        public List<ArchivedComponent> ComponentHistory { get; private set; } = [];
        public required Component CurrentComponent { get; init; }

        public required ApplicationUser Owner { get; init; }
    }
    
    internal class Component: Entity<ComponentId>
    {
        public ComponentPageId ComponentPageId { get; init; }
        
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required Version Version { get; init; }
        public required ComponentSource Source { get; init; }
    }
    
    internal class ArchivedComponent: Component
    {
        public DateTimeOffset ArchivedAt { get; init; }
    }

    [StronglyTypedId]
    internal readonly partial struct ComponentPageId;
}