using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Features.Users;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Authentication;

[HideFromDocs]
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
            // Graceful redirect with the BlazorFriendlyRedirect is not possible, because this endpoint is navigated
            // to from the Authentication Authority not our own HttpHandler and this it is not catched in the delegating handler
            return TypedResults.Redirect(("/Authentication/Login"));
        }

        var result =
            await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

        if (result.Succeeded)
        {
            var returnUrl = req.ReturnUrl == "/" ? req.ReturnUrl : "~/" + req.ReturnUrl;
            
            return TypedResults.Redirect(returnUrl);
        }

        return TypedResults.Redirect("/Authentication/Register");
    }
}

internal record ExternalLoginCallbackRequest(string ReturnUrl = "/")
{
}