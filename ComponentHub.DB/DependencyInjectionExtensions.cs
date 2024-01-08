using ComponentHub.DB.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    
    public static IServiceCollection AddEfCore(this IServiceCollection services, ConfigurationManager configurationManager, Action<DbContextOptionsBuilder>? additionalDbConfig = null)
    {
        services.AddOptions<DatabaseSettings>()
            .Bind(configurationManager.GetSection(DatabaseSettings.Section))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        var config = new DatabaseSettings();
        configurationManager.Bind(DatabaseSettings.Section, config);

        var connectionString = $"Server={config.MySql.Server}; User ID={config.MySql.Username}; Password={config.MySql.Password}; Database={config.MySql.Database}";
        return services.AddDbContextFactory<ComponentHubContext>(contextOptionsBuilder =>
        {
            contextOptionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            contextOptionsBuilder.UseOpenIddict();
            additionalDbConfig?.Invoke(contextOptionsBuilder);
        });
    }
}