using System.Net.Http.Json;
using ComponentHub.Shared.DatabaseObjects;

namespace ComponentHub.Client.Components.Features.WclComponents;

internal sealed class WclComponentService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task Upload(WclComponent component)
    {
        var response = await _httpClient.PutAsJsonAsync("api/components/upload", component);
        response.EnsureSuccessStatusCode();
    }
}