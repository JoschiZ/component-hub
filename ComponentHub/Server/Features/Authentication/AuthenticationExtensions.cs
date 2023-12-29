using ComponentHub.DB;
using ComponentHub.Domain.Features.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols.Configuration;
using OpenIddict.Abstractions;

namespace ComponentHub.Server.Features.Authentication;

internal static class AuthenticationExtensions
{

    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddAuthentication();
        services.AddAuthorization();
        
        var configurationSection = builder.Configuration.GetSection("Authentication:BattleNet");
        var secret = configurationSection["ClientSecret"];
        var clientId = configurationSection["ClientId"];
        if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(clientId))
        {
            throw new InvalidConfigurationException("ClientId or ClientSecret is missing! Check your config.");
        }

        services
            .AddIdentity<ApplicationUser, IdentityRole<UserId>>()
            .AddEntityFrameworkStores()
            .AddDefaultTokenProviders();
        
        // Maybe some Quartz code here see line 29 https://github.com/openiddict/openiddict-core/blob/dev/sandbox/OpenIddict.Sandbox.AspNetCore.Server/Startup.cs

        services.AddOpenIddict()
            .AddCore(coreBuilder =>
            {
                coreBuilder.UseEfCore();
            })
            .AddClient(clientBuilder =>
            {
                clientBuilder
                    .AllowAuthorizationCodeFlow()
                    .AllowClientCredentialsFlow();

                // Register the signing and encryption credentials used to protect
                // sensitive data like the state tokens produced by OpenIddict.
                clientBuilder
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                clientBuilder
                    .UseAspNetCore()
                    .EnableStatusCodePagesIntegration()
                    .EnableRedirectionEndpointPassthrough();

                // Register the System.Net.Http integration and use the identity of the current
                // assembly as a more specific user agent, which can be useful when dealing with
                // providers that use the user agent as a way to throttle requests (e.g Reddit).
                clientBuilder
                    .UseSystemNetHttp()
                    .SetProductInformation(typeof(Program).Assembly);
                
                
                clientBuilder.UseWebProviders()
                    .AddBattleNet(netOptions =>
                    {
                        netOptions
                            .SetClientId(clientId)
                            .SetClientSecret(secret)
                            .SetRedirectUri("api/auth/login-battlenet")
                            .AddScopes(OpenIddictConstants.Scopes.OpenId);
                    });
            });


        return builder;
    }
}