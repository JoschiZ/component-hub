using System.Net.Http.Json;
using ComponentHub.Client.Extensions;
using ComponentHub.Shared.Auth;
using ComponentHub.Shared.Helper;

namespace ComponentHub.Client.Components.Features.Auth;

internal sealed class AuthApiClient(HttpClient httpClient)
{
    public async Task Register(RegisterOptions options)
    {
        var result = await httpClient.PostAsJsonAsync("api/Auth/Register", options);
        result.EnsureSuccessStatusCode();
    }

    public async Task<LocalRedirect> Logout()
    {
        var result = await httpClient.PostWithJsonReturn<LocalRedirect>("api/Auth/Logout", null);
        return result;
    }

    public Task<UserInfo?> GetUserInfo()
    {
        return httpClient.GetFromJsonAsync<UserInfo>("api/Auth/GetUserInfo");
    }
}