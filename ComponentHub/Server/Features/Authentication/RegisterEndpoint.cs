using System.Security.Claims;
using ComponentHub.DB.Core;
using ComponentHub.Domain.Api;
using ComponentHub.Domain.Core.Primitives;
using ComponentHub.Domain.Features.Authentication;
using ComponentHub.Domain.Features.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;


namespace ComponentHub.Server.Features.Authentication;

internal sealed class RegisterEndpoint: Endpoint<RegisterRequest, IResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public RegisterEndpoint(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserStore<ApplicationUser> userStore, IUnitOfWorkFactory unitOfWorkFactory)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userStore = userStore;
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public override void Configure()
    {
        Post(Endpoints.Auth.Register);
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(RegisterRequest req, CancellationToken ct)
    {
        if (Guid.TryParse(_userManager.GetUserId(User), out var userId))
        {
            return Results.Conflict("You seem to be already registered");
        }
        
        await using var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();
        if (await unitOfWork.UserSet.AnyAsync(applicationUser => req.UserName == applicationUser.UserName, cancellationToken: ct))
        {
            return Results.Conflict("This username is already taken");
        }

        var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo is null)
        {
            return Results.Problem("Could not load the external login info");
        }

        var user = Activator.CreateInstance<ApplicationUser>();
        user.Id = UserId.New();
        await _userStore.SetUserNameAsync(user, req.UserName, CancellationToken.None);

        var createUserResult = await _userManager.CreateAsync(user);
        
        if (createUserResult.Succeeded)
        {
            // TODO Handle Login Errors, especially "Login Already Associated"
            var addLoginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
            if (addLoginResult.Succeeded)
            {
                //User.SetClaim(ClaimTypes.Name, user.UserName).SetClaim(ClaimTypes.NameIdentifier, user.Id.ToString());
                await _signInManager.SignInAsync(user, isPersistent: false, externalLoginInfo.LoginProvider);
            }
        }

        return new BlazorFriendlyRedirectResult("/");
    }
}


