using ComponentHub.DB;
using ComponentHub.Domain.Constants;
using ComponentHub.Server.Core.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Server.Features.Users;

internal sealed class DeleteAccountEndpoint: EndpointWithoutRequest<Results<Ok<AccountDeletionResponse>, UnauthorizedHttpResult>>
{
    private readonly IDbContextFactory<ComponentHubContext> _unitOfWorkFactory;

    public DeleteAccountEndpoint(IDbContextFactory<ComponentHubContext> unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public override void Configure()
    {
        Delete(Endpoints.Users.Delete);
    }

    public override async Task<Results<Ok<AccountDeletionResponse>, UnauthorizedHttpResult>> ExecuteAsync(CancellationToken ct)
    {
        var idResult = User.GetUserId();

        if (idResult.IsError)
        {
            return TypedResults.Unauthorized();
        }
        
        
        await using var context = await _unitOfWorkFactory.CreateDbContextAsync(ct);

        var deletion = await context.Users
            .Where(user => user.Id == idResult.ResultObject)
            .Take(1)
            .ExecuteDeleteAsync(ct);

        if (deletion > 0)
        {
            return TypedResults.Ok(AccountDeletionResponse.Deleted);
        }

        return TypedResults.Ok(AccountDeletionResponse.NotDeleted);
    }
}

internal sealed record AccountDeletionResponse(bool IsDeleted)
{
    public static readonly AccountDeletionResponse Deleted = new AccountDeletionResponse(true);
    public static readonly AccountDeletionResponse NotDeleted = new AccountDeletionResponse(false);
};