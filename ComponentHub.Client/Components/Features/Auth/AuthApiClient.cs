using System.Net.Http.Json;
using ComponentHub.Client.Extensions;
using ComponentHub.Domain.Api;
using ComponentHub.Domain.Core.Primitives;
using ComponentHub.Domain.Core.Primitives.Results;
using ComponentHub.Domain.Features.Authentication;
using MudBlazor;

namespace ComponentHub.Client.Components.Features.Auth;

internal sealed class AuthApiClient(HttpClient httpClient, ISnackbar snackbar)
{
    public async Task Register(RegisterRequest options)
    {
        var result = await httpClient.PostAsJsonAsync(Endpoints.Auth.Register, options);
        result.EnsureSuccessStatusCode();
    }

    public async Task<BlazorFriendlyRedirectResult> Logout()
    {
        var result = await httpClient.PostWithJsonReturn<BlazorFriendlyRedirectResult>(Endpoints.Auth.Logout, null);
        return result;
    }

    public async Task<UserInfo> GetUserInfo()
    {
        var result = await httpClient.GetFromJsonAsync<ResultError<UserInfo>>(Endpoints.Auth.GetUserInfo);
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