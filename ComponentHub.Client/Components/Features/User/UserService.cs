using ComponentHub.ApiClients.Models;
using ComponentHub.Client.ApiClients;

namespace ComponentHub.Client.Components.Features.User;

internal sealed class UserService
{
    private readonly ComponentHubClient _client;

    public UserService(ComponentHubClient client)
    {
        _client = client;
    }

    public async Task<GetDetailedUserInfoResponse> GetDetailedUserInfo()
    {
        var info = await _client.User.DetailedInfo.GetAsync();
        return info ?? new GetDetailedUserInfoResponse();
    }
}