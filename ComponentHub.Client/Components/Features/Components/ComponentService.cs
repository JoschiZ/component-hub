using ComponentHub.ApiClients.Api.Components;
using ComponentHub.ApiClients.Models;
using ComponentHub.Client.ApiClients;
using ComponentHub.Client.Core;


namespace ComponentHub.Client.Components.Features.Components;

internal sealed class ComponentService
{
    private readonly ComponentHubClient _client;
    private readonly ErrorHelper _errorHelper;

    public ComponentService(ComponentHubClient componentHubClient, ErrorHelper errorHelper)
    {
        _client = componentHubClient;
        _errorHelper = errorHelper;
    }

    public Task<CreateComponentResponse?> CreateComponent(CreateComponentRequest request)
    {
        return _client.Components.Create.PutAsync(request);
    }

    public async Task<GetComponentResponse?> GetComponent(string userName, string componentName)
    {
        try
        {
            return await _client.Components.GetPath[userName][componentName].GetAsync();
        }
        catch (Error404 e)
        {
            _errorHelper.DisplayError(e);
        }

        return default;
    }

    public async Task<List<ComponentEntryDto>> QueryComponents()
    {
        var response = await _client.Components.GetAsync(config =>
        {
            config.QueryParameters.SortDirectionAsSortDirection = SortDirection.Ascending;
        });
        return response.Components;
    }
}