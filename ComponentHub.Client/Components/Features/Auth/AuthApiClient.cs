
using ComponentHub.ApiClients.Models;
using ComponentHub.Client.ApiClients;
using MudBlazor;

namespace ComponentHub.Client.Components.Features.Auth;

internal sealed class AuthApiClient(ComponentHubClient client)
{
    public static UserInfo Empty => new UserInfo(){Id = Guid.Empty.ToString()};
    
    public async Task Register(RegisterRequest options, CancellationToken ctx)
    {
        var result = await client.Auth.Register.PostAsync(options, cancellationToken: ctx);
        if (result is null)
        {
            throw new HttpRequestException("Register Request failed");
        }
    }

    public Task Logout()
    {
        return client.Auth.Logout.PostAsync();
    }

    public async Task<UserInfo> GetUserInfo()
    {
        var result = await client.Auth.Getuserinfo.GetAsync();
        return result ?? Empty;
    }
}