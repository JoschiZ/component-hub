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

        services.AddIdentity<ApplicationUser, IdentityRole<UserId>>()
            .AddEntityFrameworkStores()
            .AddDefaultTokenProviders();
        
        // Maybe some Quartz code here see line 29 https://github.com/openiddict/openiddict-core/blob/dev/sandbox/OpenIddict.Sandbox.AspNetCore.Server/Startup.cs

        services.AddOpenIddict()
            .AddCore(coreBuilder =>
            {
                coreBuilder.UseEntityFrameworkCore()
                    .UseComponentHubContext();
            })
            .AddClient(clientBuilder =>
            {
                clientBuilder
                    .AllowAuthorizationCodeFlow()
                    .AllowClientCredentialsFlow();

                // Register the signing and encryption credentials used to protect
                // sensitive data like the state tokens produced by OpenIddict.
                clientBuilder.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                clientBuilder.UseAspNetCore()
                    .EnableStatusCodePagesIntegration()
                    .EnableRedirectionEndpointPassthrough();

                // Register the System.Net.Http integration and use the identity of the current
                // assembly as a more specific user agent, which can be useful when dealing with
                // providers that use the user agent as a way to throttle requests (e.g Reddit).
                clientBuilder.UseSystemNetHttp()
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
    
    
    /// <summary>
    /// Registers all the needed Services to allow a user to login with the BattleNet OIDC Flow.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="InvalidConfigurationException"></exception>
    public static WebApplicationBuilder AddAuthenticationOld(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        var configurationSection = builder.Configuration.GetSection("Authentication:BattleNet");
        var secret = configurationSection["ClientSecret"];
        var clientId = configurationSection["ClientId"];
        if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(clientId))
        {
            throw new InvalidConfigurationException("ClientId or ClientSecret is missing! Check your config.");
        }
        
        services
            .AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores()
            .AddDefaultTokenProviders()
            .AddSignInManager();
        
        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
            options.User.RequireUniqueEmail = false;
            options.User.AllowedUserNameCharacters += "#";
        });
        
        builder.Services.AddAuthentication(IdentityConstants.ExternalScheme)
            .AddCookie(IdentityConstants.ExternalScheme, options =>
            {
                // We can't use strict here, because FireFox won't pass on the cookie to the server
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.LoginPath = "/Authentication/Login";
                options.LogoutPath = "/Authentication/Logout";
            })
            .AddApplicationCookie();
        builder.Services.AddAuthorization();
        
        
        builder.Services.AddOpenIddict()
            .AddClient(clientBuilder =>
            {
                clientBuilder
                    .AllowAuthorizationCodeFlow()
                    .AllowRefreshTokenFlow();
                clientBuilder
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                clientBuilder
                    .UseAspNetCore()
                    .EnableRedirectionEndpointPassthrough()
                    .EnableStatusCodePagesIntegration();
                
                clientBuilder.UseWebProviders()
                    .AddBattleNet(options =>
                    {
                        options
                            .SetClientId(clientId)
                            .SetClientSecret(secret)
                            .SetRedirectUri("api/auth/login-battlenet")
                            .AddScopes(OpenIddictConstants.Scopes.OpenId);
                    });
            })
            .AddCore(coreBuilder =>
            {
                coreBuilder.UseEntityFrameworkCore()
                    .UseComponentHubContext();
            });


        return builder;
    }
}