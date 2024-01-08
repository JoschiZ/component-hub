using System.Buffers;
using ComponentHub.Domain.Core;
using ComponentHub.Domain.Features.Components;
using Microsoft.AspNetCore.Identity;
using StronglyTypedIds;

namespace ComponentHub.Domain.Features.Users;

public sealed class ApplicationUser: IdentityUser<UserId>, IAggregateRoot<UserId>
{ 
    public IEnumerable<ComponentEntry> Components { get; } = new List<ComponentEntry>();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.Now;
    
    public static class ValidationConstants
    {
        public const int MinUserNameLength = 4;
        public const int MaxUserNameLength = 24;
        public const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#";
        private static readonly SearchValues<char> AllowedCharactersSearch = SearchValues.Create(AllowedCharacters);

        public static bool IsAllowedUserName(string name)
        {
            return name.AsSpan().ContainsAnyExcept(AllowedCharactersSearch);
        }
    }
}

[StronglyTypedId]
public partial struct UserId
{
    
}