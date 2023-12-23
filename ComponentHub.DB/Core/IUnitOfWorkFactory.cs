using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ComponentHub.DB.Core;

public interface IUnitOfWorkFactory
{
    public IUnitOfWork GetUnitOfWork();
}

internal sealed class UnitOfWorkFactory: IUnitOfWorkFactory
{
    private readonly IDbContextFactory<ComponentHubContext> _contextFactory;

    public UnitOfWorkFactory(IDbContextFactory<ComponentHubContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public IUnitOfWork GetUnitOfWork()
    {
        var context = _contextFactory.CreateDbContext();
        return new UnitOfWork(context);
    }
}

