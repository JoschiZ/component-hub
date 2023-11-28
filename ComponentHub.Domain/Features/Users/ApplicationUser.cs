using ComponentHub.Domain.Features.Components;
using Microsoft.AspNetCore.Identity;
using StronglyTypedIds;

namespace ComponentHub.Domain.Features.Users;

public sealed class ApplicationUser: IdentityUser<UserId>
{ 
    public IEnumerable<Component> Components { get; } = new List<Component>();
}

[StronglyTypedId]
public partial struct UserId
{
    
}