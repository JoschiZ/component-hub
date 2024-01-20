using System.Diagnostics.CodeAnalysis;
using MudBlazor;

namespace ComponentHub.Client.Components.Helper;

/// <summary>
/// A service to provide a way to que snackbar messages from anywhere.
/// </summary>
public class SnackbarHelperService
{
    private Queue<SnackbarMessage> Messages { get; } = new Queue<SnackbarMessage>();

    public void AddMessage(SnackbarMessage message)
    {
        Messages.Enqueue(message);
        OnChange?.Invoke();
    }

    public bool GetMessage([NotNullWhen(true)] out SnackbarMessage? message)
    {
        return Messages.TryDequeue(out message);
    }

    public event Action? OnChange;
    
    
}

public record SnackbarMessage(string Message, Severity Severity = Severity.Normal, Action<SnackbarOptions>? Configure = null, string Key = "");