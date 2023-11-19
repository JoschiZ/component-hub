using ComponentHub.Shared.Features.Components;

namespace ComponentHub.Shared.Helper.Repositories;

public interface IUnitOfWork: IDisposable, IAsyncDisposable
{
        IComponentRepository Components { get; }
        int Completed();

        Task<int> CompletedAsync();
        Task<int> CompletedAsync(CancellationToken ct);
}