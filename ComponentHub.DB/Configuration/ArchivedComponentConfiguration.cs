using ComponentHub.Domain.Features.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentHub.DB.Configuration;

internal sealed class ArchivedComponentConfiguration: IEntityTypeConfiguration<ArchivedComponent>
{
    public void Configure(EntityTypeBuilder<ArchivedComponent> builder)
    {
        builder.HasKey(x => new { x.Id, x.ArchivedAt });
        //builder.Ignore(x => x.Component);
        builder.ComplexProperty<Component>(x => x.Component, propertyBuilder =>
        {
            propertyBuilder.Ignore(component => component.ComponentPage);
        });
    }
}