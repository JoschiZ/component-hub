using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Authentication;

internal sealed class ChallengeEndpoint: Endpoint<ChallengeRequest, ChallengeHttpResult>
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ChallengeEndpoint(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Post(Endpoints.Auth.ExternalLogin);
        AllowAnonymous();
        AllowFormData(urlEncoded: true);
    }

    public override Task<ChallengeHttpResult> ExecuteAsync(ChallengeRequest req, CancellationToken ct)
    {
        var returnUrl = req.ReturnUrl;
        if (string.IsNullOrEmpty(req.ReturnUrl))
        {
            returnUrl = "/";
        }

        var providerProperties = _signInManager.ConfigureExternalAuthenticationProperties(
            req.Provider,
            "external-login-callback"+ $"?ReturnUrl={returnUrl}");
        var result =
            TypedResults.Challenge(properties: providerProperties, authenticationSchemes: new[] { req.Provider });
        return Task.FromResult(result);
    }
}

internal record ChallengeRequest(string Provider, string ReturnUrl);

