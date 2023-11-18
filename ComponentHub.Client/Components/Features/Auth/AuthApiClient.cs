using System.Net.Http.Json;
using ComponentHub.Client.Extensions;
using ComponentHub.Shared.Api;
using ComponentHub.Shared.Auth;
using ComponentHub.Shared.Helper;
using ComponentHub.Shared.Results;
using MudBlazor;

namespace ComponentHub.Client.Components.Features.Auth;

internal sealed class AuthApiClient(HttpClient httpClient, ISnackbar snackbar)
{
    public async Task Register(RegisterRequest options)
    {
        var result = await httpClient.PostAsJsonAsync(Endpoints.Auth.Register, options);
        result.EnsureSuccessStatusCode();
    }

    public async Task<LocalRedirect> Logout()
    {
        var result = await httpClient.PostWithJsonReturn<LocalRedirect>(Endpoints.Auth.Logout, null);
        return result;
    }

    public async Task<UserInfo> GetUserInfo()
    {
        var result = await httpClient.GetFromJsonAsync<Result<UserInfo>>(Endpoints.Auth.GetUserInfo);
        if (result is null)
        {
            return UserInfo.Empty;
        }
        
        if (result.IsSuccess)
        {
            return result.ResultObject;
        }

        snackbar.Add("Failed to authenticate: " + result.Error.ErrorCode, Severity.Error);
        return UserInfo.Empty;
    }
}