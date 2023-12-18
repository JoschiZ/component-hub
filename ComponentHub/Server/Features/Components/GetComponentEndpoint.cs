using ComponentHub.DB.Core;
using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Server.Features.Components;

public class GetComponentEndpoint: Endpoint<GetComponentRequest, Results<Ok<ComponentDto>, NotFound, ProblemHttpResult>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetComponentEndpoint(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public override void Configure()
    {
        Get(Endpoints.Components.Get + "{UserName}" + "/" + "{ComponentName}");
    }

    public override async Task<Results<Ok<ComponentDto>, NotFound, ProblemHttpResult>> ExecuteAsync(GetComponentRequest req, CancellationToken ct)
    {
        var unit = _unitOfWorkFactory.GetUnitOfWork();
        var component = await unit.Components.QueryComponents()
            .FirstOrDefaultAsync(
                component => 
                    component.Name == req.ComponentName 
                    && component.OwnerName == req.UserName, ct);


        if (component is not null)
        {
            return ComponentDto.TryComponentToDto(component).Match<Results<Ok<ComponentDto>, NotFound, ProblemHttpResult>>(
                dto => TypedResults.Ok(dto), 
                error => error.ToProblem()
            );
        }

        return TypedResults.NotFound();
    }
}