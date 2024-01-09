using ComponentHub.Domain.Features.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentHub.DB.Configuration;

internal sealed class ComponentConfiguration: IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder)
    {
        builder.HasKey(component => component.Id);

        builder.Property<ComponentSource>(component => component.Source).HasConversion(
            source => ComponentSource.EncodeExportString(source),
            s => ComponentSource.DecodeExportString(s).ResultObject!);
    }
}