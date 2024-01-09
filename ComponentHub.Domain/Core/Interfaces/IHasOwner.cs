using ComponentHub.Domain.Features.Users;

namespace ComponentHub.Domain.Core.Interfaces;

public interface IHasOwner
{
    public UserId OwnerId { get; }
    public ApplicationUser? Owner { get; }
}