using ComponentHub.Shared.Features.Components;

namespace ComponentHub.Shared.Helper.Repositories;

internal sealed class UnitOfWork: IUnitOfWork
{
    private readonly ComponentHubContext _context;
    
    public UnitOfWork(ComponentHubContext context)
    {
        _context = context;
        Components = new ComponentRepository(_context);
    }
    
    public IComponentRepository Components { get; }
    public int Completed()
    {
        return _context.SaveChanges();
    }

    public Task<int> CompletedAsync()
    {
        return CompletedAsync(CancellationToken.None);
    }

    public Task<int> CompletedAsync(CancellationToken ct)
    {
        return _context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}