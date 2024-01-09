using ComponentHub.Domain.Features.Components;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComponentHub.DB.Configuration.Converters;

internal sealed class ComponentSourceConverter: ValueConverter<ComponentSource, string>
{
    public ComponentSourceConverter() : this(null) { }
    public ComponentSourceConverter(ConverterMappingHints mappingHints = null)
        : base(
            source =>  ComponentSource.EncodeExportString(source),
            sourceString => ComponentSource.DecodeExportString(sourceString).ResultObject!,
            mappingHints
        ) { }
}