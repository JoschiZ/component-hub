using ComponentHub.Domain.Constants;
using ComponentHub.Domain.Features.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace ComponentHub.Server.Features.Users;

internal sealed class
    ChangeUsernameEndpoint : Endpoint<ChangeUsernameEndpoint.Request, Results<Ok<ChangeUsernameEndpoint.ResponseDto>, UnauthorizedHttpResult, ProblemHttpResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangeUsernameEndpoint(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Patch(Endpoints.Users.ChangeUsername);   
    }

    public override async Task<Results<Ok<ResponseDto>, UnauthorizedHttpResult, ProblemHttpResult>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        var result = await _userManager.SetUserNameAsync(user, req.NewName);
        if (result.Succeeded)
        {
            return TypedResults.Ok(new ResponseDto(req.NewName));
        }

        return TypedResults.Problem(new ProblemDetails()
        {
            Detail = "One or more errors occurred while setting your new name: " + string.Join("; ", result.Errors.Select(error => error.Code))
        });
    }

    internal sealed record Request(string NewName);

    internal sealed record ResponseDto(string NewName);
}