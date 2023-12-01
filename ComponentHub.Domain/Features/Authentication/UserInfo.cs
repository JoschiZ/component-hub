namespace ComponentHub.Domain.Features.Authentication;

public sealed class UserInfo()
{
    public string Name { get; set; } = "";
    public bool IsAuthenticated { get; set; }
    public Dictionary<string, string> ExposedClaims { get; set; } = new();

    /// <summary>
    /// This is a guid in the backend, but a string on the principle.
    /// </summary>
    public required string Id { get; init; }
    
    /// <summary>
    /// Represents an empty user with no name, no claims and not authenticated
    /// </summary>
    public static readonly UserInfo Empty = new UserInfo(){Id = Guid.Empty.ToString()};
}