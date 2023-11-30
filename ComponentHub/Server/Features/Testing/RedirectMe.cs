using ComponentHub.Domain.Api;
using ComponentHub.Domain.Core;
using ComponentHub.Domain.Core.Primitives;

namespace ComponentHub.Server.Features.Testing;

internal sealed class RedirectMeEndpoint : Endpoint<RedirectMeRequest, BlazorFriendlyRedirectResult>
{
    public override void Configure()
    {
        Post(Endpoints.Testing.RedirectMe);
        AllowAnonymous();
    }

    public async override Task<BlazorFriendlyRedirectResult> ExecuteAsync(RedirectMeRequest req, CancellationToken ct)
    {
        return new BlazorFriendlyRedirectResult(){Route = req.Path};
    }
}


