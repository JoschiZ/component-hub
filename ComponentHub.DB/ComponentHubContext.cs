using ComponentHub.Domain.Features.Components;
using ComponentHub.Domain.Features.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.DB;

public sealed class ComponentHubContext: IdentityDbContext<ApplicationUser, IdentityRole<UserId>, UserId>
{
    public DbSet<ComponentEntry> Components { get; set; } = default!;

    public ComponentHubContext(DbContextOptions<ComponentHubContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ComponentHubContext).Assembly);
        
        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<UserId>().HaveConversion<UserId.EfCoreValueConverter>();
        configurationBuilder.Properties<ComponentEntryId>().HaveConversion<ComponentEntryId.EfCoreValueConverter>();
        configurationBuilder.Properties<ComponentId>().HaveConversion<ComponentId.EfCoreValueConverter>();
        configurationBuilder.Properties<CommentId>().HaveConversion<CommentId.EfCoreValueConverter>();
        configurationBuilder.Properties<Version>().HaveConversion<VersionConverter>();
        
        base.ConfigureConventions(configurationBuilder);
    }
}