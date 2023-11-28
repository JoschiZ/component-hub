using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Components;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ComponentHub.Server.Features.Components;

internal sealed class
    UpdateComponentEndpoint : Endpoint<UpdateComponentRequest, Ok>
{
    public override void Configure()
    {
        Patch(Endpoints.Components.Update);       
    }


    public override Task<Ok> ExecuteAsync(UpdateComponentRequest req, CancellationToken ct)
    {
        //TODO
        throw new NotImplementedException();
        return base.ExecuteAsync(req, ct);
    }
}
