using System.Net;
using System.Net.Http.Json;
using ComponentHub.ApiClients.Models;
using ComponentHub.Client.Components.Helper;
using ComponentHub.Domain.Core.Primitives;
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

        try
        {
            var deserializedError = await response.Content.ReadFromJsonAsync<BaseError>(cancellationToken: cancellationToken);
            _snackbarHelperService.AddMessage(new SnackbarMessage(deserializedError?.Message ?? "Unknown Error", Severity.Error));
        }
        catch (Exception)
        {
            _snackbarHelperService.AddMessage(new SnackbarMessage(await response.Content.ReadAsStringAsync(cancellationToken), Severity.Error));
        }

        return response;
    }
}