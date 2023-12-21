using ComponentHub.Domain.Features.Components;
using Riok.Mapperly.Abstractions;


namespace ComponentHub.Server.Features.Components;

internal readonly record struct ComponentEntryDto(
    ComponentEntryId Id,
    string Name,
    string Description,
    string OwnerName,
    DateTime CreationDate,
    DateTime UpdatedAt
);


[Mapper]
internal static partial class ComponentEntryMapper
{
    public static partial ComponentEntryDto ToDto(this ComponentEntry entry);
}

