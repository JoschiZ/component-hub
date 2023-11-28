using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Users;
using FastEndpoints;
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
        Post(Endpoints.Auth.Challenge);
        AllowAnonymous();
        AllowFormData(urlEncoded: true);
    }

    public override Task<ChallengeHttpResult> ExecuteAsync(ChallengeRequest req, CancellationToken ct)
    {
        var providerProperties = _signInManager.ConfigureExternalAuthenticationProperties(
            req.Provider,
            null);
        return Task.FromResult(TypedResults.Challenge(properties: providerProperties, authenticationSchemes: new[] {req.Provider}));
    }
}

internal record ChallengeRequest(string Provider);

