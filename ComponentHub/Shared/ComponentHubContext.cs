using ComponentHub.Shared.DatabaseObjects;
using ComponentHub.Shared.Features.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Shared.Auth;

public sealed class ComponentHubContext(DbContextOptions<ComponentHubContext> options): IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ComponentHubContext).Assembly);
        base.OnModelCreating(builder);
    }
}