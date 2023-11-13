namespace ComponentHub.Shared.Results;

public sealed class Error(string errorCode, string? description)
{
    public string? Description { get; } = description;
    public string ErrorCode { get; } = errorCode;

    public static readonly Error UserNotFoundError = new("UserNotFound", "Could Not Find the User");
}