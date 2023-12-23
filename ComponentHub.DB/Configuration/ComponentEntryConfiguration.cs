using ComponentHub.Domain.Features.Components;
using ComponentHub.Domain.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentHub.DB.Configuration;

internal sealed class ComponentEntryConfiguration: IEntityTypeConfiguration<ComponentEntry>
{
    public void Configure(EntityTypeBuilder<ComponentEntry> builder)
    {
        builder.HasKey(entry => entry.Id);

        // TODO: Index for restriction of Name Per User?

        builder
            .HasMany<Component>(entry => entry.ComponentHistory)
            .WithOne(component => component.ComponentEntry)
            .HasForeignKey(component => component.ComponentEntryId)
            .IsRequired();

        builder
            .HasMany<Comment>(entry => entry.Comments)
            .WithOne(comment => comment.ComponentEntry)
            .HasForeignKey(comment => comment.ComponentEntryId);

        builder
            .HasOne<ApplicationUser>(entry => entry.Owner)
            .WithMany(user => user.Components)
            .IsRequired();

    }
}