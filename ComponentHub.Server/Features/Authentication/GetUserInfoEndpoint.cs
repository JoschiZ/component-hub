using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Features.Authentication;
using ComponentHub.Domain.Features.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Authentication;

internal sealed class GetUserInfoEndpoint: EndpointWithoutRequest<Ok<UserInfo>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserInfoEndpoint(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Get(Endpoints.Auth.GetUserInfo);
        AllowAnonymous();
    }

    public override Task<Ok<UserInfo>> ExecuteAsync(CancellationToken ct)
    {
        if (User.Identity?.Name is null)
        {
            return Task.FromResult(TypedResults.Ok(UserInfo.Empty));
        }

        var userId = _userManager.GetUserId(User);

        if (userId is null)
        {
            return Task.FromResult(TypedResults.Ok(UserInfo.Empty));
        }
        var userInfo = new UserInfo()
        {
            IsAuthenticated = User.Identity.IsAuthenticated,
            Name = User.Identity.Name,
            ExposedClaims = User.Claims
                .ToDictionary(c => c.Type, c => c.Value),
            Id = userId
        };

        return Task.FromResult(TypedResults.Ok(userInfo));
    }
}