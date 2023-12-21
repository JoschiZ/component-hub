using System.Security.Claims;
using ComponentHub.ApiClients.Models;
using ComponentHub.Client.Core;
using Microsoft.AspNetCore.Components.Authorization;

namespace ComponentHub.Client.Components.Features.Auth;

internal sealed class IdentityAuthenticationStateProvider(
    AuthApiClient authClient,
    RedirectHelper redirectHelper
    ): AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var userInfo = await authClient.GetUserInfo();
        var identity = new ClaimsIdentity();
        
        if (userInfo.IsAuthenticated != null && (userInfo == AuthApiClient.Empty || !userInfo.IsAuthenticated.Value))
        {
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        if (userInfo.Name is null || userInfo.ExposedClaims is null)
        {
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userInfo.Name)
        }.Concat(userInfo.ExposedClaims.AdditionalData.Select(claim => new Claim(claim.Key, claim.Value.ToString())));
        identity = new ClaimsIdentity(claims, "Server Authentication");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task Register(RegisterRequest options, CancellationToken ctx)
    {
        await authClient.Register(options, ctx);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        redirectHelper.Redirect("/");
    }
    
    public async Task Logout()
    {
        await authClient.Logout();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}