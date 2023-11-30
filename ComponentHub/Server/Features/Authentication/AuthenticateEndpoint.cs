using System.Security.Claims;
using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;

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
        return BattleNetCallback();
    }

    public async Task<IResult> BattleNetCallback()
    {
        var result = await HttpContext.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);
        
        if (result.Principal is not ClaimsPrincipal { Identity.IsAuthenticated: true })
        {
            throw new InvalidOperationException("The external authorization data cannot be used for authentication.");
        }
        
        // Build an identity based on the external claims and that will be used to create the authentication cookie.
        var identity = new ClaimsIdentity(authenticationType: "ExternalLogin");
        
        
        identity
            .SetClaim(ClaimTypes.Name, result.Principal.GetClaim("battle_tag"))
            .SetClaim(ClaimTypes.NameIdentifier, result.Principal.GetClaim(ClaimTypes.NameIdentifier));
        
        
        // Preserve the registration details to be able to resolve them later.
        identity
            .SetClaim(OpenIddictConstants.Claims.Private.RegistrationId, result.Principal!.GetClaim(OpenIddictConstants.Claims.Private.RegistrationId))
            .SetClaim(OpenIddictConstants.Claims.Private.ProviderName, result.Principal!.GetClaim(OpenIddictConstants.Claims.Private.ProviderName));
        
        
        // Build the authentication properties based on the properties that were added when the challenge was triggered.
        var properties = new AuthenticationProperties(result.Properties?.Items ?? new Dictionary<string, string?>())
        {
            RedirectUri = result.Properties?.RedirectUri ?? "/"
        };
        
        return TypedResults.SignIn(new ClaimsPrincipal(identity), properties);
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

