using ComponentHub.Domain.Core.Primitives.Results;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ComponentHub.Server.Core;

internal static class ResultExtensions
{
    public static ProblemHttpResult ToResult(this Error error)
    {
        return TypedResults.Problem(title: error.ErrorCode, detail: error.Description);
    }
}