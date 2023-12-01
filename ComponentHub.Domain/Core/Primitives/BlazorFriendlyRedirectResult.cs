using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace ComponentHub.Domain.Core.Primitives;

/// <summary>
/// A custom HTTP 310 response, that is used to trigger non reloading graceful reloads in the blazor WASM client.
/// </summary>
public sealed class BlazorFriendlyRedirectResult: IResult
{
    public BlazorFriendlyRedirectResult(string route = "/")
    {
        Route = route;
        RedirectType = RedirectType.Simple;
    }

    public BlazorFriendlyRedirectResult(string route, Dictionary<string, string?> query)
    {
        Route = route;
        Query = query;
        RedirectType = RedirectType.WithQuery;
    }

    [JsonConstructor]
    public BlazorFriendlyRedirectResult(string route, Dictionary<string, string?> query, RedirectType redirectType)
    {
        Route = route;
        Query = query;
        RedirectType = redirectType;
    }
    
    public const int HttpStatusCode = 310;
    public RedirectType RedirectType { get; set; }

    public Dictionary<string, string?> Query { get; set; } = new();
    public string Route { get; set; }
    public Task ExecuteAsync(HttpContext httpContext)
    {
        var jsonResponse = TypedResults.Json<BlazorFriendlyRedirectResult>(this, JsonSerializerOptions.Default, "application/json", HttpStatusCode);
        return jsonResponse.ExecuteAsync(httpContext);
    }
}

public enum RedirectType
{
    Simple,
    WithQuery
}