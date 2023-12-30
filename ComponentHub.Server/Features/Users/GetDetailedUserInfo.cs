using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Components;
using ComponentHub.Domain.Features.Users;
using ComponentHub.Server.Core.ResponseObjects;
using ComponentHub.Server.Features.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Users;

internal sealed class GetDetailedUserInfo : EndpointWithoutRequest<Results<Ok<GetDetailedUserInfoResponse>, NotFound<Error404>, UnauthorizedHttpResult>>
{
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly UserManager<ApplicationUser> _userManager;


    public GetDetailedUserInfo(IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager)
    {
        _userStore = userStore;
        _userManager = userManager;
    }

    public override void Configure()
    {
        Get(Endpoints.Users.GetDetailedInfo);
    }

    public override async Task<Results<Ok<GetDetailedUserInfoResponse>, NotFound<Error404>, UnauthorizedHttpResult>> ExecuteAsync(CancellationToken ct)
    {

        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return TypedResults.Unauthorized();
        }
        
        var user = await _userStore.FindByIdAsync(userId, ct);

        if (user is null)
        {
            return TypedResults.NotFound(new Error404());
        }


        var detailedInfo = new GetDetailedUserInfoResponse(user.UserName!, user.CreatedAt, user.Id);
        return TypedResults.Ok(detailedInfo);
    }
}

internal sealed record GetDetailedUserInfoRequest();

internal sealed record GetDetailedUserInfoResponse(
    string Name,
    DateTimeOffset CreationDate,
    UserId UserId);

