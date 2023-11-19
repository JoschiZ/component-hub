using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentHub.Shared.Helper.Repositories;

public static class RepositoriesExtensions
{
    public static IServiceCollection UseRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<ComponentHubContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlite("Filename=app.db").UseOpenIddict();
        });
        return services;
    }

    public static IdentityBuilder AddEntityFrameworkStores(this IdentityBuilder builder)
    {
        builder.AddEntityFrameworkStores<ComponentHubContext>();
        return builder;
    }

    public static OpenIddictEntityFrameworkCoreBuilder UseComponentHubContext(this OpenIddictEntityFrameworkCoreBuilder builder)
    {
        builder.UseDbContext<ComponentHubContext>();
        return builder;
    }
}