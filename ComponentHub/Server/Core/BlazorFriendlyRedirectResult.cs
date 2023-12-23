using System.Text.Json;
using System.Text.Json.Serialization;
using ComponentHub.Domain.Core;

namespace ComponentHub.Server.Core;

/// <summary>
/// A custom HTTP 210 response, that is used to trigger non reloading graceful reloads in the blazor WASM client.
/// </summary>
public sealed class BlazorFriendlyRedirectResult: IResult
{
    public BlazorFriendlyRedirectResult(string route = "/")
    {
        RedirectInfo = new RedirectInfo()
        {
            Route = route,
            RedirectType = RedirectType.Simple
        };
    }

    public BlazorFriendlyRedirectResult(string route, Dictionary<string, string?> query)
    {
        RedirectInfo = new RedirectInfo()
        {
            Route = route,
            Query = query,
            RedirectType = RedirectType.WithQuery
        };
    }

    [JsonConstructor]
    public BlazorFriendlyRedirectResult(string route, Dictionary<string, string?> query, RedirectType redirectType)
    {
        RedirectInfo = new RedirectInfo()
        {
            Route = route,
            Query = query,
            RedirectType = redirectType
        };
    }
    
    public const int HttpStatusCode = 210;
    
    public RedirectInfo RedirectInfo { get; }
    public Task ExecuteAsync(HttpContext httpContext)
    {
        var jsonResponse = TypedResults.Json(RedirectInfo, JsonSerializerOptions.Default, "application/json", HttpStatusCode);
        return jsonResponse.ExecuteAsync(httpContext);
    }
}

