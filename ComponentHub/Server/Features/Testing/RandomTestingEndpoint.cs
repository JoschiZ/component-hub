using System.ComponentModel;
using ComponentHub.Server.Features.Components;

namespace ComponentHub.Server.Features.Testing;

internal sealed class RandomTestingEndpoint: Endpoint<RandomTestingRequest>
{
    public override void Configure()
    {
        Get("RandomTestingEndpoint");
    }

    public override Task<object?> ExecuteAsync(RandomTestingRequest req, CancellationToken ct)
    {
        return base.ExecuteAsync(req, ct);
    }
}

internal sealed record RandomTestingRequest(NestedRecord NestedRecord);

internal sealed record NestedRecord(string SomeProp, [property: DefaultValue("Default from Attribute")]string SomePropWithDefault = "I'm a default");