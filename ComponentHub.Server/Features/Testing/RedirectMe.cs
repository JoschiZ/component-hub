using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Core;
using ComponentHub.Domain.Core.Primitives;
using ComponentHub.Server.Core;

namespace ComponentHub.Server.Features.Testing;

internal sealed class RedirectMeEndpoint : Endpoint<RedirectMeRequest, BlazorFriendlyRedirectResult>
{
    public override void Configure()
    {
        Post(Endpoints.Testing.RedirectMe);
        AllowAnonymous();
    }

    public override Task<BlazorFriendlyRedirectResult> ExecuteAsync(RedirectMeRequest req, CancellationToken ct)
    {
        return Task.FromResult(new BlazorFriendlyRedirectResult(req.Path));
    }
}


