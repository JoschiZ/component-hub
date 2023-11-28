using System.Security.Claims;
using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Authentication;
using ComponentHub.Domain.Features.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;


namespace ComponentHub.Server.Features.Authentication;

internal sealed class RegisterEndpoint: Endpoint<RegisterRequest, IResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserStore<ApplicationUser> _userStore;

    public RegisterEndpoint(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserStore<ApplicationUser> userStore)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userStore = userStore;
    }

    public override void Configure()
    {
        Post(Endpoints.Auth.Register);
    }

    public override async Task<IResult> ExecuteAsync(RegisterRequest req, CancellationToken ct)
    {
        if (Guid.TryParse(_userManager.GetUserId(User), out var _) && await _userManager.GetUserAsync(User) is not null)
        {
            return Results.Conflict("You are already registered");
        }
        
        
        var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo is null)
        {
            return Results.Problem("Could not load the external login info");
        }
        


        var user = Activator.CreateInstance<ApplicationUser>();
        await _userStore.SetUserNameAsync(user, req.UserName, CancellationToken.None);

        var createUserResult = await _userManager.CreateAsync(user);
        
        if (createUserResult.Succeeded)
        {
            var addLoginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
            if (addLoginResult.Succeeded)
            {
                User.SetClaim(ClaimTypes.Name, user.UserName).SetClaim(ClaimTypes.NameIdentifier, user.Id.ToString());
                await _signInManager.SignInAsync(user, isPersistent: false, externalLoginInfo.LoginProvider);
            }
        }

        return Results.SignIn(User, new AuthenticationProperties(){RedirectUri = "/"});
    }
}


