using System.Net;
using System.Net.Http.Json;
using System.Text;
using MudBlazor;

namespace ComponentHub.Client.Core;

internal sealed class ValidationDelegatingHandler: DelegatingHandler
{
    private readonly ISnackbar _snackbar;
    
    public ValidationDelegatingHandler(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.BadRequest) return response;
        try
        {
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

            _snackbar.Add("sb.ToString()", Severity.Error);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return response;
    }
}