using ComponentHub.Domain.Core;
using ComponentHub.Domain.Features.Components;
using Microsoft.AspNetCore.Identity;
using StronglyTypedIds;

namespace ComponentHub.Domain.Features.Users;

public sealed class ApplicationUser: IdentityUser<UserId>, IAggregateRoot<UserId>
{ 
    public IEnumerable<ComponentEntry> Components { get; } = new List<ComponentEntry>();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.Now;
}

[StronglyTypedId]
public partial struct UserId
{
    
}