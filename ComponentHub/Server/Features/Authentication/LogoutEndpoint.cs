using ComponentHub.Shared.Api;
using ComponentHub.Shared.Auth;
using ComponentHub.Shared.DatabaseObjects;
using ComponentHub.Shared.Helper;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Authentication;

internal sealed class LogoutEndpoint: EndpointWithoutRequest<LocalRedirect>
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutEndpoint(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Post(Endpoints.Auth.Logout);
    }

    public override async Task<LocalRedirect> ExecuteAsync(CancellationToken ct)
    {
        await _signInManager.SignOutAsync();
        return new LocalRedirect()
        {
            Route = "/"
        };
    }
}