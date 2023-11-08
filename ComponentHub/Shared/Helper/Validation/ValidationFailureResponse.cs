namespace ComponentHub.Shared.Helper.Validation;

public sealed class ValidationFailureResponse
{
    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
}