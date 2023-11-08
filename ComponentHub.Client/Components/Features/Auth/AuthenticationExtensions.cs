using ComponentHub.Client.Components.Features.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ComponentHub.Client.Features.Auth;

internal static class AuthenticationExtensions
{
    public static WebAssemblyHostBuilder AddAuthentication(this WebAssemblyHostBuilder builder)
    {
        var services = builder.Services;

        services.AddScoped<AuthApiClient>();
        
        builder.Services.AddScoped<IdentityAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(
            s => s.GetRequiredService<IdentityAuthenticationStateProvider>());

        services.AddAuthorizationCore();


        return builder;
    }
}