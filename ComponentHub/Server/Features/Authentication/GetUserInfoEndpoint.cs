using ComponentHub.Domain.Api;
using ComponentHub.Domain.Core.Primitives.Results;
using ComponentHub.Domain.Features.Authentication;
using ComponentHub.Domain.Features.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Authentication;

internal sealed class GetUserInfoEndpoint: EndpointWithoutRequest<ResultError<UserInfo>>
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

    public async override Task<ResultError<UserInfo>> ExecuteAsync(CancellationToken ct)
    {
        if (User.Identity?.Name is null)
        {
            return UserInfo.Empty;
        }

        var userId = _userManager.GetUserId(User);

        if (userId is null)
        {
            return Error.UserNotFoundError;
        }
        
        return new UserInfo(userId)
        {
            IsAuthenticated = User.Identity.IsAuthenticated,
            Name = User.Identity.Name,
            ExposedClaims = User.Claims
                .ToDictionary(c => c.Type, c => c.Value),
        };
    }
}