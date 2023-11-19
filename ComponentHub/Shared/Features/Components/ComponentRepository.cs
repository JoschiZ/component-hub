using ComponentHub.Shared.Helper.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Shared.Features.Components;

internal sealed class ComponentRepository(ComponentHubContext context) : Repository<Component, ComponentId>(context), IComponentRepository
{
    public IEnumerable<Component> GetAllComponentsOfUser(Guid userId)
    {
        return _dbSet.Include(c => c.Owner).Where(component => component.Owner.Id == userId).ToList();
    }
}