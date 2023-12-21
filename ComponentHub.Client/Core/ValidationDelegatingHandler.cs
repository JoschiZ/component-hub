using System.Net;
using System.Net.Http.Json;
using System.Text;
using ComponentHub.Client.Components.Helper;
using MudBlazor;

namespace ComponentHub.Client.Core;

internal sealed class ValidationDelegatingHandler: DelegatingHandler
{
    private readonly SnackbarHelperService _helperServiceHelper;
    
    public ValidationDelegatingHandler(SnackbarHelperService helperService)
    {
        _helperServiceHelper = helperService;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.BadRequest) return response;

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cancellationToken) ?? new ProblemDetails();

        var sb = new StringBuilder();
        sb.Append("Validation Error");
        foreach (var detail in problemDetails.Errors)
        {
            sb.Append(detail.Name);
            sb.Append(": ");
            sb.Append(detail.Reason);
            sb.Append('\n');
        }

        _helperServiceHelper.AddMessage(new SnackbarMessage(sb.ToString(), Severity.Error));

        return response;
    }
}