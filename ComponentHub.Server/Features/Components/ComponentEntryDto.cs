using System.Diagnostics.CodeAnalysis;
using ComponentHub.Domain.Features.Components;
using Riok.Mapperly.Abstractions;


namespace ComponentHub.Server.Features.Components;

internal record ComponentPageDto(
    ComponentPageId Id,
    string Name,
    string Description,
    string OwnerName,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);



[Mapper]
[SuppressMessage("ReSharper", "RedundantVerbatimPrefix")]
internal static partial class ComponentEntryMapper
{
    [MapProperty(nameof(@ComponentPage.Owner.UserName), nameof(ComponentPageDto.OwnerName))]
    public static partial ComponentPageDto ToDto(this ComponentPage page);
    
    public static partial IQueryable<ComponentPageDto> ProjectToDto(this IQueryable<ComponentPage> entry);
}

