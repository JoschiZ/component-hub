using System.Text.Json;
using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Features.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Server.Features.Users;

internal sealed class DownloadUserdataEndpoint : EndpointWithoutRequest<Results<FileStreamHttpResult, UnauthorizedHttpResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DownloadUserdataEndpoint(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    public override void Configure()
    {
        Get(Endpoints.Users.DownloadUserdata);
    }

    public override async Task<Results<FileStreamHttpResult, UnauthorizedHttpResult>> ExecuteAsync(CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        var json = JsonSerializer.SerializeToUtf8Bytes(user);
        var ms = new MemoryStream(json);

        return TypedResults.File(ms, "application/json", fileDownloadName: "userData.json");
    }
}