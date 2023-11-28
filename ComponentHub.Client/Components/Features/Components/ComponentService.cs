using System.Net.Http.Json;
using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Components;

namespace ComponentHub.Client.Components.Features.Components;

internal sealed class ComponentService
{
    private readonly HttpClient _httpClient;

    public ComponentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async void CreateComponent(CreateComponentRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(Endpoints.Components.Create, request);
        
    }
}