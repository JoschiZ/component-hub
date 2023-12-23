using ComponentHub.Domain.Features.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentHub.DB.Configuration;

internal sealed class ApplicationUserConfiguration: IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(user => user.Id);
        builder.HasIndex(user => user.UserName).IsUnique();
    }
}

