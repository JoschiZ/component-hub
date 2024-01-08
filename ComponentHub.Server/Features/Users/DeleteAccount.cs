using ComponentHub.DB.Core;
using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Features.Users;
using ComponentHub.Server.Core.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Server.Features.Users;

internal sealed class DeleteAccountEndpoint: EndpointWithoutRequest<Results<Ok<AccountDeletionResponse>, UnauthorizedHttpResult>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public DeleteAccountEndpoint(IUnitOfWorkFactory unitOfWorkFactory)
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
        
        
        await using var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var deletion = await unitOfWork.UserSet.Where(user => user.Id == idResult.ResultObject)
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