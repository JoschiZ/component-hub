using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Users;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Authentication;

internal sealed class ExternalLoginCallbackEndpoint: Endpoint<ExternalLoginCallbackRequest, IResult>
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ExternalLoginCallbackEndpoint(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Get(Endpoints.Auth.ExternalLoginCallback);
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(ExternalLoginCallbackRequest req, CancellationToken ct)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            // TODO Replace with a graceful redirect?
            return TypedResults.Redirect("/Authentication/Login");
        }

        var result =
            await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

        if (result.Succeeded)
        {
            return TypedResults.Redirect(req.ReturnUrl);
        }

        return TypedResults.Redirect("/Authentication/Register");
    }
}

internal record ExternalLoginCallbackRequest(string ReturnUrl = "/")
{
}