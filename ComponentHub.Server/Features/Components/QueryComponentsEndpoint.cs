using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ComponentHub.DB.Core;
using ComponentHub.Domain.Api;
using ComponentHub.Domain.Features.Components;
using ComponentHub.Server.Core;
using ComponentHub.Server.Core.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NJsonSchema.Annotations;

namespace ComponentHub.Server.Features.Components;

internal sealed class
    QueryComponentsEndpoint : Endpoint<QueryComponentsEndpointRequest, Ok<QueryComponentsEndpointResponse>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public QueryComponentsEndpoint(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public override void Configure()
    {
        Get(Endpoints.Components.Query);
        AllowAnonymous();
    }

    public override async Task<Ok<QueryComponentsEndpointResponse>> ExecuteAsync(QueryComponentsEndpointRequest req, CancellationToken ct)
    {
        await using var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var componentsQuery = unitOfWork.Components.Query()
            .Include(entry => entry.Owner)
            .Where(entry => entry.Owner.UserName!.Contains(req.UserName) && entry.Name.Contains(req.ComponentName));
        var orderAction = new ComponentsSortAction(req.SortDirection, req.SortingMethod).GetOrderMethod();
        var componentsOrderedQuery = orderAction(componentsQuery)
            .ThenBy(entry => entry.Name)
            .Skip(req.PageSize * req.Page)
            .Take(req.PageSize)
            .Select(entry => entry.ToDto());

        var components = await componentsOrderedQuery.ToArrayAsync(cancellationToken: ct);
            
        return TypedResults.Ok(new QueryComponentsEndpointResponse(components));
    }
    
}

internal sealed record QueryComponentsEndpointRequest(
    string UserName = "",
    string ComponentName = "",
    SortDirection SortDirection = SortDirection.Ascending,
    SortingMethod SortingMethod = SortingMethod.ByName,
    int Page = 0,
    int PageSize = 10);

internal sealed record QueryComponentsEndpointResponse(ComponentEntryDto[] Components);

internal sealed class QueryComponentsEndpointRequestValidator: Validator<QueryComponentsEndpointRequest>
{
    public QueryComponentsEndpointRequestValidator()
    {
        RuleFor(request => request.PageSize).InclusiveBetween(1, 20);
        RuleFor(request => request.Page).GreaterThanOrEqualTo(0);
    }
}

internal sealed record ComponentsSortAction(SortDirection SortDirection, SortingMethod Method)
{
    public Func<IQueryable<ComponentEntry>, IOrderedQueryable<ComponentEntry>> GetOrderMethod()
    {
        return Method switch
        {
            SortingMethod.ByUserName => query =>
                query.OrderByWithDirection(SortDirection, entry => entry.Owner.UserName),
            SortingMethod.ByName => query => query.OrderByWithDirection(SortDirection, entry => entry.Name),
            SortingMethod.ByCreationDate => query =>
                query.OrderByWithDirection(SortDirection, entry => entry.CreatedAt),
            SortingMethod.ByUpdateDate => query => query.OrderByWithDirection(SortDirection, entry => entry.UpdatedAt),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
};

internal enum SortDirection
{
    Ascending,
    Descending
}

internal enum SortingMethod
{
    ByUserName,
    ByName,
    ByCreationDate,
    ByUpdateDate,
}