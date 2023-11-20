using FastEndpoints.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Server.Tests.Endpoints;

public sealed class EndpointIntegrationFixture: TestFixture<Program>
{
    public EndpointIntegrationFixture(IMessageSink s) : base(s)
    {
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        
        base.ConfigureServices(s);
    }
}