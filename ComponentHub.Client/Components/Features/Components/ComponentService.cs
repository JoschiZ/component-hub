using ComponentHub.ApiClients.Models;
using ComponentHub.Client.ApiClients;
using ComponentHub.Client.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.Kiota.Abstractions;


namespace ComponentHub.Client.Components.Features.Components;

internal sealed class ComponentService
{
    private readonly ComponentHubClient _client;
    private readonly ErrorHelper _errorHelper;
    private readonly NavigationManager _navigationManager;

    public ComponentService(ComponentHubClient componentHubClient, ErrorHelper errorHelper, NavigationManager navigationManager)
    {
        _client = componentHubClient;
        _errorHelper = errorHelper;
        _navigationManager = navigationManager;
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
            _navigationManager.NavigateTo(Routes.General.NotFound, replace: true);
        }

        return default;
    }

    public async Task<List<ComponentEntryDto>> GetByUser(string userName, CancellationToken ct)
    {
        try
        {
            return await _client.Components[userName].GetAsync(null, ct) ?? [];
        }
        catch (ApiException e)
        {
            _errorHelper.DisplayError(e);
        }

        return [];
    }

    public async Task<List<ComponentEntryDto>> QueryComponents()
    {
        var response = await _client.Components.GetAsync(config =>
        {
            config.QueryParameters.SortDirectionAsSortDirection = SortDirection.Ascending;
        });
        return response?.Components ?? [];
    }

    public async Task Delete(string componentEntryId)
    {
        try
        {
            await _client.Components.DeletePath.DeleteAsync(new DeleteComponentRequest() { ComponentId = componentEntryId });
        }
        catch (ApiException e)
        {
            _errorHelper.DisplayError(e);
        }
    }
}