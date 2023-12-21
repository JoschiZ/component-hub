using System.Net;
using ComponentHub.Client.Components.Helper;
using MudBlazor;

namespace ComponentHub.Client.Core;

internal class ErrorDelegatingHandler: DelegatingHandler
{
    private readonly SnackbarHelperService _snackbarHelperService;

    public ErrorDelegatingHandler(SnackbarHelperService snackbarHelperService)
    {
        _snackbarHelperService = snackbarHelperService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        // Bad Requests are handled seperately as validation errros
        if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
        {
            return response;
        }

        _snackbarHelperService.AddMessage(new SnackbarMessage(await response.Content.ReadAsStringAsync(cancellationToken), Severity.Error));

        return response;
    }
}