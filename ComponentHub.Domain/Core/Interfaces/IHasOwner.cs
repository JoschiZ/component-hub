using ComponentHub.Domain.Features.Users;

namespace ComponentHub.Domain.Core.Interfaces;

public interface IHasOwner
{
    public ApplicationUser Owner { get; }
}