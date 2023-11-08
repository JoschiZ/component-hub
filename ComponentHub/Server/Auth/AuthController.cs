using System.Security.Claims;
using ComponentHub.Server.Database;
using ComponentHub.Shared.Auth;
using ComponentHub.Shared.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Client.WebIntegration;

namespace ComponentHub.Server.Auth;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class AuthController(
    SignInManager<ApplicationUser> signInManager,
    IUserStore<ApplicationUser> userStore,
    UserManager<ApplicationUser> userManager)
    : ControllerBase
{

    
    [HttpPost]
    public IResult Login([FromForm] string provider)
    {
        var providerProperties = signInManager.ConfigureExternalAuthenticationProperties(
            provider,
            null
        );
        return Results.Challenge(properties: providerProperties, authenticationSchemes: new[] {provider});
    }

    /// <summary>
    /// OidC requires this to be a Get request even though it will change the Server state
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<IResult> LoginCallbackBattleNet()
    {
        return LoginUser(OpenIddictClientWebIntegrationConstants.Providers.BattleNet);
    }

    private async Task<IResult> LoginUser(string provider)
    {
        // Retrieve the authorization data validated by OpenIddict as part of the callback handling.
        var result = await HttpContext.AuthenticateAsync(provider);

        var battleTag = result.Principal!.GetClaim("battle_tag");
        var battleNetId = result.Principal!.GetClaim(ClaimTypes.NameIdentifier);
        
        var identity = new ClaimsIdentity(authenticationType: IdentityConstants.ExternalScheme);


        if (string.IsNullOrEmpty(battleTag) || string.IsNullOrEmpty(battleNetId))
        {
            return Results.Problem("Could not parse your battlenet information");
        }
        
        identity
            .SetClaim(ClaimTypes.Name, battleTag)
            .SetClaim(ClaimTypes.NameIdentifier, battleNetId);

        // Preserve the registration details to be able to resolve them later.
        identity
            .SetClaim(OpenIddictConstants.Claims.Private.RegistrationId, result.Principal!.GetClaim(OpenIddictConstants.Claims.Private.RegistrationId))
            .SetClaim(OpenIddictConstants.Claims.Private.ProviderName, result.Principal!.GetClaim(OpenIddictConstants.Claims.Private.ProviderName));

        // Build the authentication properties based on the properties that were added when the challenge was triggered.
        var properties = new AuthenticationProperties(result.Properties.Items);
        
        var tryExternalSignIn = await signInManager.ExternalLoginSignInAsync(
            provider,
            battleNetId,
            isPersistent: false,
            bypassTwoFactor: true
        );

        if (!tryExternalSignIn.Succeeded)
        {
            var redirectUrl = UriHelper.BuildRelative(
                HttpContext.Request.PathBase,
                "/Authentication/Register");
            properties.RedirectUri = redirectUrl;
        }

        properties.RedirectUri ??= "/";

        
        return Results.SignIn(new ClaimsPrincipal(identity), properties);
    }
    
    
    [HttpPost]
    public async Task<IResult> Register(RegisterOptions options)
    {
        var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo is null)
        {
            return Results.Problem("Could not load the external login info");
        }
        
        var tryExternalSignIn = await signInManager.ExternalLoginSignInAsync(
            externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true
        );

        if (tryExternalSignIn.Succeeded)
        {
            return await LoginUser("BattleNet");
        }

        var user = Activator.CreateInstance<ApplicationUser>();
        await userStore.SetUserNameAsync(user, options.UserName, CancellationToken.None);

        var createUserResult = await userManager.CreateAsync(user);
        
        if (createUserResult.Succeeded)
        {
            var addLoginResult = await userManager.AddLoginAsync(user, externalLoginInfo);
            if (addLoginResult.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false, externalLoginInfo.LoginProvider);
            }
        }

        return Results.Redirect("/");
    }

    [HttpGet]
    public UserInfo GetUserInfo()
    {
        if (User.Identity?.Name is null)
        {
            return new UserInfo()
            {
                Name = "UserNotFound",
            };
        }
        
        return new UserInfo
        {
            IsAuthenticated = User.Identity.IsAuthenticated,
            Name = User.Identity.Name,
            ExposedClaims = User.Claims
                .ToDictionary(c => c.Type, c => c.Value)
        };
    }

    [Authorize]
    [HttpPost]
    public async Task<LocalRedirect> Logout()
    {
        await signInManager.SignOutAsync();
        return new LocalRedirect()
        {
            Route = "/"
        };
    }
    
    
}