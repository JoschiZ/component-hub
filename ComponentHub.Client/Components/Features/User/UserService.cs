using ComponentHub.ApiClients.Models;
using ComponentHub.Client.ApiClients;
using ComponentHub.Client.Components.Features.Auth;
using ComponentHub.Client.Core;
using Microsoft.AspNetCore.Components;

namespace ComponentHub.Client.Components.Features.User;

internal sealed class UserService
{
    private readonly ComponentHubClient _client;
    private readonly NavigationManager _navigationManager;
    private readonly ErrorHelper _errorHelper;
    private readonly AuthApiClient _authApiClient;

    public UserService(ComponentHubClient client, ErrorHelper errorHelper, NavigationManager navigationManager, AuthApiClient authApiClient)
    {
        _client = client;
        _errorHelper = errorHelper;
        _navigationManager = navigationManager;
        _authApiClient = authApiClient;
    }

    public async Task<GetDetailedUserInfoResponse> GetDetailedUserInfo()
    {
        try
        {
            var info = await _client.User.DetailedInfo.GetAsync();
            return info ?? new GetDetailedUserInfoResponse();
        }
        catch (Error404)
        {
            return new GetDetailedUserInfoResponse();
        }
    }

    public async Task<AccountDeletionResponse?> DeleteAccount(CancellationToken cancellationToken)
    {
        var response = await _client.User.DeletePath.DeleteAsync(null, cancellationToken);
        await _authApiClient.Logout();
        return response;
    }
}