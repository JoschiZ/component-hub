using FluentValidation.Results;

namespace ComponentHub.Domain.Core.Primitives.Results;

public class Error(string errorCode, string? description)
{
    public string? Description { get; } = description;
    public string ErrorCode { get; } = errorCode;

    public static readonly Error UserNotFoundError = new("UserNotFound", "Could Not Find the Users");

    public static readonly Error InvalidExportString = new Error("ComponentSource.InvalidString",
        "The provided string could not be decoded into a valid source");
}

public sealed class ValidationError(string errorCode, string? description, IEnumerable<ValidationFailure> validationFailures) : Error(errorCode, description)
{
    public IEnumerable<ValidationFailure> ValidationFailures { get; } = validationFailures;
}