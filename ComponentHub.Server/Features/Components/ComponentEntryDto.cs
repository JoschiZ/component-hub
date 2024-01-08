using ComponentHub.Domain.Features.Components;
using Riok.Mapperly.Abstractions;


namespace ComponentHub.Server.Features.Components;

internal record ComponentEntryDto(
    ComponentEntryId Id,
    string Name,
    string Description,
    string OwnerName,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);

/// <summary>
/// A Minimal DTO that only carries essential information
/// </summary>
/// <param name="Name"></param>
/// <param name="OwnerName"></param>
/// <param name="Id"></param>
internal record ComponentEntryMinimalDto(string Name, string OwnerName, ComponentEntryId Id);



[Mapper]
internal static partial class ComponentEntryMapper
{
    [MapProperty(nameof(@ComponentEntry.Owner.UserName), nameof(ComponentEntryDto.OwnerName))]
    public static partial ComponentEntryDto ToDto(this ComponentEntry entry);
    
    [MapProperty(nameof(@ComponentEntry.Owner.UserName), nameof(ComponentEntryMinimalDto.OwnerName))]
    public static partial ComponentEntryMinimalDto ToMinimalDto(this ComponentEntry entry);
}

