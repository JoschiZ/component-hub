using ComponentHub.Shared.Features.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StronglyTypedIds;

namespace ComponentHub.Shared.DatabaseObjects;

public sealed class ApplicationUser: IdentityUser<Guid>
{ 
    public ICollection<Component> Components { get; } = new List<Component>();
}

internal sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasMany<Component>(o => o.Components).WithOne(o => o.Owner);
    }
}
