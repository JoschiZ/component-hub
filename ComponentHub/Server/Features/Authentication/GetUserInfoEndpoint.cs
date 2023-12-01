using ComponentHub.Domain.Api;
using ComponentHub.Domain.Core.Primitives.Results;
using ComponentHub.Domain.Features.Authentication;
using ComponentHub.Domain.Features.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Authentication;

internal sealed class GetUserInfoEndpoint: EndpointWithoutRequest<Results<Ok<UserInfo>, UnauthorizedHttpResult>>
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

    public async override Task<Results<Ok<UserInfo>, UnauthorizedHttpResult>> ExecuteAsync(CancellationToken ct)
    {
        if (User.Identity?.Name is null)
        {
            return TypedResults.Unauthorized();
        }

        var userId = _userManager.GetUserId(User);

        if (userId is null)
        {
            return TypedResults.Unauthorized();
        }
        var userInfo = new UserInfo()
        {
            IsAuthenticated = User.Identity.IsAuthenticated,
            Name = User.Identity.Name,
            ExposedClaims = User.Claims
                .ToDictionary(c => c.Type, c => c.Value),
            Id = userId
        };

        return TypedResults.Ok(userInfo);
    }
}