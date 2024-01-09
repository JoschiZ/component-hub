using ComponentHub.Domain.Core;
using ComponentHub.Domain.Features.Users;
using StronglyTypedIds;

namespace ComponentHub.Domain.Features.Components;

public class Comment : Entity<CommentId>
{
    public DateTime TimeStamp { get; }
    public string Content { get; }
    
    
    // Relations
    public UserId OwnerId { get; init; }
    public ApplicationUser? Owner { get; }
    
    public ComponentPageId ComponentPageId { get; init; }
    public ComponentPage ComponentPage { get; } 
}

[StronglyTypedId]
public readonly partial struct CommentId{}