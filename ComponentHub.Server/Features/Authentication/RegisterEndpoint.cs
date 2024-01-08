using ComponentHub.DB;
using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Features.Users;
using ComponentHub.Server.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ComponentHub.Server.Features.Authentication;

internal sealed class RegisterEndpoint: Endpoint<RegisterRequest, Results<Conflict<string>, BlazorFriendlyRedirectResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IDbContextFactory<ComponentHubContext> _contextFactory;


    public RegisterEndpoint(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserStore<ApplicationUser> userStore, IDbContextFactory<ComponentHubContext> contextFactory)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userStore = userStore;
        _contextFactory = contextFactory;
    }

    public override void Configure()
    {
        Post(Endpoints.Auth.Register);
        AllowAnonymous();
    }

    public override async Task<Results<Conflict<string>, BlazorFriendlyRedirectResult>> ExecuteAsync(RegisterRequest req, CancellationToken ct)
    {
        if (Guid.TryParse(_userManager.GetUserId(User), out var userId))
        {
            return TypedResults.Conflict("You seem to be already registered");
        }

        await using var context = await _contextFactory.CreateDbContextAsync(ct);

        if (await context.Users.AnyAsync(applicationUser => req.UserName == applicationUser.UserName, cancellationToken: ct))
        {
            return TypedResults.Conflict("This username is already taken");
        }

        var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo is null)
        {
            return TypedResults.Conflict("Could not load the external login info");
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


