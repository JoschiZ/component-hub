using System.Security.Claims;
using ComponentHub.Domain.Core.Primitives.Results;
using ComponentHub.Domain.Features.Components;
using ComponentHub.Domain.Features.Users;
using FluentValidation.Results;
using OneOf;

namespace ComponentHub.DB.Core;

public interface IComponentEntryRepository: IRepository<ComponentEntry, ComponentEntryId>
{
    IQueryable<ComponentEntry> Query();
    Task<OneOf<ComponentEntry, Error, List<ValidationFailure>>> UpdateComponent(UserId userId, ComponentEntryId reqEntryId, ComponentId reqComponentId, string reqName, string reqSourceCode, short reqHeight, short reqWidth, Guid reqWclComponentId, CancellationToken ct);

    Task<int> RemoveByIdAsync(ComponentEntryId id, ClaimsPrincipal principal, CancellationToken ct);
}