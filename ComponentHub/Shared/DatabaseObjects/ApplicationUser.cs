using ComponentHub.Shared.Components;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.Shared.Auth;

public sealed class ApplicationUser: IdentityUser<Guid>
{
 //   public ICollection<Component> Components { get; } = new List<Component>();
}