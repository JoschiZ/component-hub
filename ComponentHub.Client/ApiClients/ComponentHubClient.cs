using ComponentHub.ApiClients;
using ComponentHub.ApiClients.Api.Auth;
using ComponentHub.ApiClients.Api.Components;
using ComponentHub.ApiClients.Api.Testing;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace ComponentHub.Client.ApiClients;

internal class ComponentHubClient
{
    private readonly ComponentHubBaseClient _client;

    public AuthRequestBuilder Auth => _client.Api.Auth;
    public ComponentsRequestBuilder Components => _client.Api.Components;
    public TestingRequestBuilder Testing => _client.Api.Testing;

    public ComponentHubClient(HttpClient httpClient)
    {

        var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: httpClient);
        _client = new ComponentHubBaseClient(adapter);
    }
}