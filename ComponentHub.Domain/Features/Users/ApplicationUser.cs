using ComponentHub.DB.Features.Components;
using Microsoft.AspNetCore.Identity;

namespace ComponentHub.DB.Features.User;

public sealed class ApplicationUser: IdentityUser<Guid>
{ 
    public ICollection<Component> Components { get; } = new List<Component>();
}