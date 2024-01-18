using ComponentHub.Domain.Features.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentHub.DB.Configuration;

internal sealed class ComponentTagConfiguration: IEntityTypeConfiguration<ComponentTag>
{
    public void Configure(EntityTypeBuilder<ComponentTag> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).UsePropertyAccessMode(PropertyAccessMode.Property);

        // seed the database with all enum members
        builder.HasData(ComponentTagIdExtensions.GetValues().Select(id => new ComponentTag(id)));
    }
}