using System.Runtime.InteropServices.JavaScript;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ComponentHub.Domain.Core.Primitives.Results;

public sealed class Error(string errorCode, string? description)
{
    public string? Description { get; } = description;
    public string ErrorCode { get; } = errorCode;

    public ProblemHttpResult ToProblem()
    {
        return TypedResults.Problem(title: ErrorCode, detail: Description);
    }
    
    public static readonly Error UserNotFoundError = new("UserNotFound", "Could Not Find the Users");

    public static readonly Error InvalidExportString = new Error("ComponentSource.InvalidString",
        "The provided string could not be decoded into a valid source");
}
