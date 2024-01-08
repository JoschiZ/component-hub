using Microsoft.AspNetCore.Components;

namespace ComponentHub.Client.Core;

public abstract class CancellationService: IDisposable, IAsyncDisposable
{
    private readonly NavigationManager _navigationManager;
    
#pragma warning disable Ex0104
    public CancellationTokenSource Cts { get; } = new();
#pragma warning restore Ex0104

    public CancellationToken Token => Cts.Token;

    protected CancellationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _navigationManager.LocationChanged += (_, _) => Cts.Cancel();
    }

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await Cts.CancelAsync();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}