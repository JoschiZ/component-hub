using ComponentHub.Shared.DatabaseObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Shared;


internal sealed class ComponentHubContext(DbContextOptions<ComponentHubContext> options): IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ComponentHubContext).Assembly);
        base.OnModelCreating(builder);
    }
}