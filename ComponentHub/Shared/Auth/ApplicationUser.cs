using ComponentHub.Shared.DatabaseObjects;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Shared.Auth;

public sealed class ApplicationUser: IdentityUser<Guid>
{
    public ICollection<WclComponent> Components { get; } = new List<WclComponent>();
}