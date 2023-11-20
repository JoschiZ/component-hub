using ComponentHub.Shared.Components;
using ComponentHub.Shared.DatabaseObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentHub.Shared.Features.Components;

public sealed class ComponentConfiguration : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder)
    {
        builder.HasKey(component => component.Id);
        //builder.Property(component => component.Id).HasConversion(id => id.Value, guid => new ComponentId(guid));
        builder.Property(component => component.Id).HasConversion<ComponentId.EfCoreValueConverter>();

        builder.ComplexProperty(component => component.Source, propertyBuilder =>
        {
            propertyBuilder.Property(source => source.SourceCode).HasMaxLength(ComponentSource.Validator.MaxSourceLength);
            propertyBuilder.IsRequired();
        });
        builder.HasOne<ApplicationUser>(e => e.Owner).WithMany(e => e.Components);
    }
}