using ComponentHub.Domain.Features.Components;
using Riok.Mapperly.Abstractions;

namespace ComponentHub.Server.Features.Components;

internal record ComponentDto(
    ComponentId Id,
    string Name,
    string ComponentSource,
    Version Version);
    
[Mapper]
internal static partial class ComponentDtoMapper
{
    [MapProperty(nameof(Component.Source), nameof(ComponentDto.ComponentSource))]
    public static partial ComponentDto ToDto(this Component component);

    private static string ComponentSourceToCompressed(ComponentSource componentSource)
    {
        return ComponentSource.EncodeExportString(componentSource);
    }
}