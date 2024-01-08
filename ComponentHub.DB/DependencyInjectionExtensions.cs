using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentHub.DB;

public static class DependencyInjectionExtensions
{
    public static IdentityBuilder AddEntityFrameworkStores(this IdentityBuilder builder)
    {
        builder.AddEntityFrameworkStores<ComponentHubContext>();
        return builder;
    }

    public static OpenIddictEntityFrameworkCoreBuilder UseEfCore(this OpenIddictCoreBuilder builder)
    {
        return builder
            .UseEntityFrameworkCore()
            .UseDbContext<ComponentHubContext>();
    }

    public static IServiceCollection AddEfCore(this IServiceCollection services, Action<DbContextOptionsBuilder>? config = null)
    {
        return services.AddDbContextFactory<ComponentHubContext>(builder =>
        {
            builder.UseSqlite("Filename=app.db");
            builder.UseOpenIddict();
            config?.Invoke(builder);
        });
    }
}