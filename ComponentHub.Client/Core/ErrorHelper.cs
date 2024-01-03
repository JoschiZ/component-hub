using ComponentHub.Client.Components.Helper;
using Microsoft.Kiota.Abstractions;
using MudBlazor;

namespace ComponentHub.Client.Core;

internal sealed partial class ErrorHelper
{
    private readonly SnackbarHelperService _snackbarHelper;
    private readonly ILogger<ErrorHelper> _logger;

    [LoggerMessage(
        Message = "An API Exception occurred: {Message}",
        Level = LogLevel.Error)]
    private partial void LogApiException(string message);

    public ErrorHelper(SnackbarHelperService snackbarHelper, ILogger<ErrorHelper> logger)
    {
        _snackbarHelper = snackbarHelper;
        _logger = logger;
    }

    public void DisplayError(ApiException exception, Severity severity = Severity.Error)
    {
        _snackbarHelper.AddMessage(new SnackbarMessage(exception.Message, severity));
        LogApiException(exception.Message);
        
    }
}