using ComponentHub.DB;
using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Features.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace ComponentHub.Server.Features.Users;

internal sealed class DeleteAccountEndpoint: EndpointWithoutRequest<Results<Ok<AccountDeletionResponse>, UnauthorizedHttpResult, ProblemHttpResult>>
{
    private readonly IDbContextFactory<ComponentHubContext> _unitOfWorkFactory;
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteAccountEndpoint(IDbContextFactory<ComponentHubContext> unitOfWorkFactory, UserManager<ApplicationUser> userManager)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _userManager = userManager;
    }

    public override void Configure()
    {
        Delete(Endpoints.Users.Delete);
    }

    public override async Task<Results<Ok<AccountDeletionResponse>, UnauthorizedHttpResult, ProblemHttpResult>> ExecuteAsync(CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            return TypedResults.Ok(AccountDeletionResponse.Deleted);
        }

        return TypedResults.Problem(new ProblemDetails()
        {
            Detail = string.Join("; ", result.Errors.Select(error => error.Code))
        });
    }
}

internal sealed record AccountDeletionResponse(bool IsDeleted)
{
    public static readonly AccountDeletionResponse Deleted = new AccountDeletionResponse(true);
};