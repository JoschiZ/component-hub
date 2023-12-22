using ComponentHub.ApiClients.Models;
using ComponentHub.Client.ApiClients;





namespace ComponentHub.Client.Components.Features.Components;

internal sealed class ComponentService
{
    private readonly ComponentHubClient _client;

    public ComponentService(ComponentHubClient componentHubClient)
    {
        _client = componentHubClient;
    }

    public Task<CreateComponentResponse?> CreateComponent(CreateComponentRequest request)
    {
        return _client.Components.Create.PutAsync(request);
    }

    public Task<GetComponentResponse?> GetComponent(string componentName, string userName)
    {
        return _client.Components.GetPath[userName][componentName].GetAsync();

    }
}