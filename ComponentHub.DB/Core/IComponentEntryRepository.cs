using ComponentHub.Domain.Features.Components;
using ComponentHub.Domain.Features.Users;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using Error = ComponentHub.Domain.Core.Primitives.Results.Error;

namespace ComponentHub.DB.Core;

public interface IComponentEntryRepository: IRepository<ComponentEntry, ComponentEntryId>
{
    IQueryable<ComponentEntry> Query();
    Task<OneOf<ComponentEntry, Error, List<ValidationFailure>>> UpdateComponent(UserId userId, ComponentEntryId reqEntryId, ComponentId reqComponentId, string reqName, string reqSourceCode, short reqHeight, short reqWidth, Guid reqWclComponentId, CancellationToken ct);
}

internal sealed class ComponentEntryRepository: Repository<ComponentEntry, ComponentEntryId>, IComponentEntryRepository
{
    public ComponentEntryRepository(ComponentHubContext context) : base(context)
    {
    }

    public IQueryable<ComponentEntry> Query()
    {
        return Set.AsQueryable();
    }

    public async Task<OneOf<ComponentEntry, Error, List<ValidationFailure>>> UpdateComponent(
        UserId userId,
        ComponentEntryId reqEntryId,
        ComponentId reqComponentId,
        string reqName,
        string reqSourceCode,
        short reqHeight,
        short reqWidth,
        Guid reqWclComponentId,
        CancellationToken ct)
    {
        var root = await Set
            .Where(entry => entry.Owner.Id == userId)
            .FirstOrDefaultAsync(entry => entry.Id == reqEntryId, cancellationToken: ct);

        if (root is null)
        {
            return Error.UserNotFoundError;
        }

        var currentComponent = root.GetCurrentComponent();

        var updatedSourceResult = currentComponent.Source.TryGetUpdatedSource(reqSourceCode, reqHeight, reqWidth, reqWclComponentId);

        if (updatedSourceResult.IsError)
        {
            return updatedSourceResult.Error;
        }

        var updatedSource = updatedSourceResult.ResultObject;

        var updatedComponentResult = Component.TryCreate(updatedSource, reqName, root.Id, root);

        if (updatedComponentResult.IsError)
        {
            return updatedComponentResult.Error;
        }

        var updatedComponent = updatedComponentResult.ResultObject;

        root.UpdateCurrentComponent(updatedComponent);

        return root;
    }
}