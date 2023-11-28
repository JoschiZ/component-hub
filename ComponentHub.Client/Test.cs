using MudBlazor;

namespace ComponentHub.Client;

internal sealed class Test
{
    private readonly ISnackbar _snackbar;

    public Test(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    public Task ShowSnackbar()
    {
        _snackbar.Add("Hello", Severity.Error);
        return Task.CompletedTask;
    }
}