namespace ComponentHub.Domain.Core;

/// <summary>
/// A Helper class containing Information that is used to perform local blazor WASM redirects
/// </summary>
public sealed class RedirectInfo
{
    public RedirectType RedirectType { get; set; }

    public Dictionary<string, string?> Query { get; set; } = new();
    public required string Route { get; set; }
}

public enum RedirectType
{
    Simple,
    WithQuery
}