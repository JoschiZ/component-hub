using Microsoft.AspNetCore.Components;

namespace ComponentHub.Client.Core;

public interface ICancellationService : IDisposable, IAsyncDisposable
{
    CancellationTokenSource Cts { get; }
    CancellationToken Token { get; }
}

public sealed class CancellationService: ICancellationService
{
    private readonly NavigationManager _navigationManager;
    

    public CancellationTokenSource Cts { get; } = new();


    public CancellationToken Token => Cts.Token;

    public CancellationService(NavigationManager navigationManager)
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