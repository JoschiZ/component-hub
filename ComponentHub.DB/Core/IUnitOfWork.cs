using ComponentHub.Domain.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.DB.Core;

public interface IUnitOfWork: IDisposable, IAsyncDisposable
{
    public IComponentEntryRepository Components { get; }
    public DbSet<ApplicationUser> UserSet { get; }
    Task<int> CompletedAsync(CancellationToken ct);

    public void Attach<TItem>(TItem item) where TItem: notnull;
}

internal sealed class UnitOfWork: IUnitOfWork
{
    private readonly ComponentHubContext _context;

    public UnitOfWork(ComponentHubContext context)
    {
        _context = context;
        Components = new ComponentEntryRepository(context);
        UserSet = _context.Users;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    public IComponentEntryRepository Components { get; }
    public DbSet<ApplicationUser> UserSet { get; }

    public Task<int> CompletedAsync(CancellationToken ct)
    {
        return _context.SaveChangesAsync(ct);
    }

    public void Attach<TItem>(TItem item) where TItem: notnull => _context.Attach(item);
}