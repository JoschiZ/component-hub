using ComponentHub.DB.BaseClasses;
using ComponentHub.DB.Features.User;
using ComponentHub.Shared;
using ComponentHub.Shared.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols.Configuration;
using OpenIddict.Abstractions;

namespace ComponentHub.Server.Auth;

internal static class AuthenticationExtensions
{
    /// <summary>
    /// Registers all the needed Services to allow a user to login with the BattleNet OIDC Flow.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="InvalidConfigurationException"></exception>
    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
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