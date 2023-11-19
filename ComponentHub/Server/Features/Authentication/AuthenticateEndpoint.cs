using System.Security.Claims;
using ComponentHub.Shared.Api;
using ComponentHub.Shared.Auth;
using ComponentHub.Shared.DatabaseObjects;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace ComponentHub.Server.Features.Authentication;

internal sealed class AuthenticateEndpoint: EndpointWithoutRequest<IResult>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticateEndpoint(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public override void Configure()
    {
        Get(Endpoints.Auth.Authenticate);
        AllowAnonymous();
    }

    public override Task<IResult> ExecuteAsync(CancellationToken ct)
    {
        return LoginUser("BattleNet");
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

        // Preserve the registration details to be able to resolve them later.
        identity
            .SetClaim(OpenIddictConstants.Claims.Private.RegistrationId, result.Principal!.GetClaim(OpenIddictConstants.Claims.Private.RegistrationId))
            .SetClaim(OpenIddictConstants.Claims.Private.ProviderName, result.Principal!.GetClaim(OpenIddictConstants.Claims.Private.ProviderName));


        var tryExternalSignIn = await _signInManager.ExternalLoginSignInAsync(
            provider,
            battleNetId,
            isPersistent: false,
            bypassTwoFactor: true
        );


        var propertyItems = result.Properties is not null ? result.Properties.Items : new Dictionary<string, string?>();
        // Build the authentication properties based on the properties that were added when the challenge was triggered.
        var properties = new AuthenticationProperties(propertyItems);

        if (!tryExternalSignIn.Succeeded)
        {
            var redirectUrl = UriHelper.BuildRelative(
                HttpContext.Request.PathBase,
                "/Authentication/Register");
            properties.RedirectUri = redirectUrl;
        }

        var user = await _userManager.FindByLoginAsync(provider, battleNetId);
        if (user is not null)
        {
            identity.SetClaim(ClaimTypes.Name, user.UserName).SetClaim(ClaimTypes.NameIdentifier, user.Id.ToString());
        }
        else
        {
            identity.SetClaim(ClaimTypes.Name, battleTag).SetClaim(ClaimTypes.NameIdentifier, battleNetId);
        }
        

        properties.RedirectUri ??= "/";

        //return Results.Ok();
        return Results.SignIn(new ClaimsPrincipal(identity), properties);
    }
}

