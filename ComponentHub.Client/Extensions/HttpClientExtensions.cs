using System.Net;
using System.Net.Http.Json;

namespace ComponentHub.Client.Extensions;

internal static class HttpClientExtensions
{
    public static async Task<TResult> PostWithJsonReturn<TResult>(this HttpClient httpClient, string uri, HttpContent? httpContent)
    {
        var response = await httpClient.PostAsync(uri, httpContent);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TResult>();;
        if (result is null)
        {
            throw new Exception( "Could not correctly deserialize"+result);
        }

        return result;
    }

    public static async Task<TResult> PostAsJsonWithJsonReturnAsync<TResult, TOptions>(this HttpClient httpClient, string uri, TOptions options)
    {
        var response = await httpClient.PostAsJsonAsync(uri, options);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TResult>();;
        if (result is null)
        {
            throw new Exception( "Could not correctly deserialize"+result);
        }

        return result;
    }
}