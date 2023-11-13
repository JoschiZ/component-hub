using ComponentHub.Shared.DatabaseObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Shared.Auth;

public sealed class ComponentHubContext(DbContextOptions<ComponentHubContext> options): IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<WclComponent> Components { get; } = default!;
}