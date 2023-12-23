using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComponentHub.DB;

internal sealed class VersionConverter: ValueConverter<Version, string>
{
    public VersionConverter() : this(null) { }
    public VersionConverter(ConverterMappingHints mappingHints = null)
        : base(
            version => version.ToString(),
            versionString => Version.Parse(versionString),
            mappingHints
        ) { }
}