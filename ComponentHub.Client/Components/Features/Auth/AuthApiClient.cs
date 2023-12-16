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
        if ((int)result.StatusCode != 310)
        {
            result.EnsureSuccessStatusCode();    
        }
        
    }

    public Task Logout()
    {
        return httpClient.PostAsync(Endpoints.Auth.Logout, null);
        
    }

    public async Task<UserInfo> GetUserInfo()
    {
        var result = await httpClient.GetAsync(Endpoints.Auth.GetUserInfo);

        if (!result.IsSuccessStatusCode)
        {
            return UserInfo.Empty;
        }

        var userInfo = await result.Content.ReadFromJsonAsync<UserInfo>();
        
        return userInfo ?? UserInfo.Empty;
    }
}