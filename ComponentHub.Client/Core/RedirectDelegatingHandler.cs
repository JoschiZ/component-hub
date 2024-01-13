using System.Net.Http.Json;
using ComponentHub.Domain.Core;
using ComponentHub.Domain.Core.Primitives;

namespace ComponentHub.Client.Core;

internal class RedirectDelegatingHandler: DelegatingHandler
{
    private readonly RedirectHelper _redirectHelper;

    public RedirectDelegatingHandler(RedirectHelper redirectHelper)
    {
        _redirectHelper = redirectHelper;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        if ((int)response.StatusCode != 210)
        {
            return response;
        }

        var redirect = await response.Content.ReadFromJsonAsync<RedirectInfo>(cancellationToken: cancellationToken);

        if (redirect is null)
        {
            return response;
        }
        
        _redirectHelper.Redirect(redirect);

        return response;
    }
}