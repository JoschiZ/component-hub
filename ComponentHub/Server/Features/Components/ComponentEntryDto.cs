using ComponentHub.Domain.Features.Components;
using Riok.Mapperly.Abstractions;


namespace ComponentHub.Server.Features.Components;

internal readonly record struct ComponentEntryDto(
    ComponentEntryId Id,
    string Name,
    string Description,
    string OwnerName,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);


[Mapper]
internal static partial class ComponentEntryMapper
{
    [MapProperty(nameof(@ComponentEntry.Owner.UserName), nameof(ComponentEntryDto.OwnerName))]
    public static partial ComponentEntryDto ToDto(this ComponentEntry entry);
}

