using ComponentHub.Domain.Features.Components;
using ComponentHub.Domain.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentHub.DB.Configuration;

internal sealed class ComponentPageConfiguration: IEntityTypeConfiguration<ComponentPage>
{
    public void Configure(EntityTypeBuilder<ComponentPage> builder)
    {
        builder.HasKey(entry => entry.Id);

        builder.HasIndex(page => new { page.OwnerId, page.Name }).IsUnique();

        builder
            .HasMany(entry => entry.ComponentHistory)
            .WithOne(component => component.ComponentPage)
            .HasForeignKey(component => component.ComponentPageId)
            .IsRequired();

        builder
            .HasMany(entry => entry.Comments)
            .WithOne(comment => comment.ComponentPage)
            .HasForeignKey(comment => comment.ComponentPageId);

        builder
            .HasOne<ApplicationUser>(entry => entry.Owner)
            .WithMany(user => user.Components)
            .HasForeignKey(page => page.OwnerId)
            .IsRequired();

        builder
            .HasOne<Component>(page => page.Component)
            .WithOne(component => component.ComponentPage)
            .HasForeignKey<Component>(component => component.ComponentPageId)
            .IsRequired();

        builder
            .HasMany(page => page.Tags)
            .WithMany(tag => tag.Pages);

        builder.Navigation(page => page.Component).AutoInclude();

    }
}