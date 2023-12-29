using ComponentHub.Client.Components.Helper;
using Microsoft.Kiota.Abstractions;
using MudBlazor;

namespace ComponentHub.Client.Core;

internal sealed class ErrorHelper
{
    private readonly SnackbarHelperService _snackbarHelper;

    public ErrorHelper(SnackbarHelperService snackbarHelper)
    {
        _snackbarHelper = snackbarHelper;
    }

    public void DisplayError(ApiException exception, Severity severity = Severity.Error)
    {
        _snackbarHelper.AddMessage(new SnackbarMessage(exception.Message, severity));
    }
}