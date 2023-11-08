namespace ComponentHub.Shared.Auth;

public sealed class UserInfo
{
    public string Name { get; set; } = "";
    public bool IsAuthenticated { get; set; }
    public Dictionary<string, string> ExposedClaims { get; set; } = new Dictionary<string, string>();
}