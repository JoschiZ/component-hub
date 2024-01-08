using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Core.Primitives;
using ComponentHub.Domain.Features.Users;
using ComponentHub.Server.Core;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Authentication;


internal sealed class LogoutEndpoint: EndpointWithoutRequest<BlazorFriendlyRedirectResult>
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

    public override async Task<BlazorFriendlyRedirectResult> ExecuteAsync(CancellationToken ct)
    {
        await _signInManager.SignOutAsync();
        return new BlazorFriendlyRedirectResult("/");
    }
}