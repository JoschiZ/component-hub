namespace ComponentHub.Shared.Auth;

public sealed class UserInfo(string id)
{
    public string Name { get; set; } = "";
    public bool IsAuthenticated { get; set; }
    public Dictionary<string, string> ExposedClaims { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// This is a guid in the backend, but a string on the principle.
    /// </summary>
    public string Id { get; init; } = id;


    /// <summary>
    /// Represents an empty user with no name, no claims and not authenticated
    /// </summary>
    public static readonly UserInfo Empty = new UserInfo(Guid.Empty.ToString());
}