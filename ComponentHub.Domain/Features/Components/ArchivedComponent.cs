using ComponentHub.Domain.Core;

namespace ComponentHub.Domain.Features.Components;

public sealed class ArchivedComponent: Entity<ComponentId>
{
    public required DateTimeOffset ArchivedAt { get; init; }
    public required Component Component { get; init; }
    public required ComponentPageId ComponentPageId { get; init; }
    public ComponentPage? ComponentPage { get; init; }
    
    private ArchivedComponent(ComponentId id) : base(id)
    {
    }

    public static ArchivedComponent Create(Component component)
    {
        return new ArchivedComponent(component.Id)
        {
            Component = component,
            ArchivedAt = DateTimeOffset.Now,
            ComponentPageId = component.ComponentPageId,
            ComponentPage = component.ComponentPage
        };
    }
}
