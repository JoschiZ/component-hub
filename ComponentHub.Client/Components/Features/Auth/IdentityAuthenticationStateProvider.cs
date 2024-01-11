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
    public UserInfo? CurrentUser { get; set; }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (CurrentUser is null)
        { 
            CurrentUser = await authClient.GetUserInfo();
        }
        
        var identity = new ClaimsIdentity();
        
        if (CurrentUser.IsAuthenticated != null && (CurrentUser == AuthApiClient.Empty || !CurrentUser.IsAuthenticated.Value))
        {
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        if (CurrentUser.Name is null || CurrentUser.ExposedClaims is null)
        {
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, CurrentUser.Name)
        }.Concat(CurrentUser.ExposedClaims.AdditionalData.Select(claim => new Claim(claim.Key, claim.Value.ToString())));
        identity = new ClaimsIdentity(claims, "Server Authentication");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task Register(RegisterRequest options, CancellationToken ctx)
    {
        await authClient.Register(options, ctx);
        AuthStateHasChanged();
        redirectHelper.Redirect("/");
    }
    
    public async Task Logout()
    {
        await authClient.Logout();
        AuthStateHasChanged();
    }

    public void Login()
    {
        //Void the current user cache for a new login.
        // The rest is done as a form request
        CurrentUser = null;
    }

    public void AuthStateHasChanged()
    {
        CurrentUser = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public bool IsCurrentUser(string userName)
    {
        if (CurrentUser is null)
        {
            return false;
        }

        return CurrentUser.Name == userName;
    }
}