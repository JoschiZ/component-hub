using ComponentHub.Domain.Core.Primitives.Results;
using OneOf;

namespace ComponentHub.Domain.Features.Components;

public record struct ComponentDto(string Name, ComponentSource Source, string OwnerName)
{
    public static OneOf<ComponentDto, Error> TryComponentToDto(Component component)
    {
        return new ComponentDto(component.Name, component.Source, component.OwnerName);
    }
};