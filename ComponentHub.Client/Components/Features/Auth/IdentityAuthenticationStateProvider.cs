using System.Security.Claims;
using ComponentHub.Shared.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace ComponentHub.Client.Components.Features.Auth;

internal sealed class IdentityAuthenticationStateProvider(
    AuthApiClient authClient,
    RedirectHelper.RedirectHelper redirectHelper
    ): AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var userInfo = await authClient.GetUserInfo();
        var identity = new ClaimsIdentity();
        if (userInfo == UserInfo.Empty || !userInfo.IsAuthenticated)
        {
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userInfo.Name)
        }.Concat(userInfo.ExposedClaims.Select(claim => new Claim(claim.Key, claim.Value)));
        identity = new ClaimsIdentity(claims, "Server Authentication");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task Register(RegisterRequest options)
    {
        await authClient.Register(options);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        redirectHelper.Redirect("/");
    }
    
    public async Task Logout()
    {
        var result = await authClient.Logout();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        redirectHelper.Redirect(result);
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}