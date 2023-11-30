using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ComponentHub.Domain.Core.Primitives;

/// <summary>
/// A custom HTTP 310 response, that is used to trigger non reloading graceful reloads in the blazor WASM client.
/// </summary>
public sealed class BlazorFriendlyRedirectResult: IResult
{
    public const int HttpStatusCode = 310;
    public RedirectType RedirectType { get; set; } = RedirectType.Simple;
    public string Route { get; set; } = "/";
    public Task ExecuteAsync(HttpContext httpContext)
    {
        var jsonResponse = TypedResults.Json<BlazorFriendlyRedirectResult>(this, JsonSerializerOptions.Default, "application/json", HttpStatusCode);
        return jsonResponse.ExecuteAsync(httpContext);
    }
}

public enum RedirectType
{
    Simple,
    WithMessage
}