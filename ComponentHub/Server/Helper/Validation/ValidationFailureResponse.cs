using ComponentHub.Shared.Helper.Validation;
using FluentValidation.Results;

namespace ComponentHub.Server.Helper.Validation;

internal static class ValidationFailureMapper
{
    public static ValidationFailureResponse ToResponse(this IEnumerable<ValidationFailure> failures)
    {
        return new ValidationFailureResponse()
        {
            Errors = failures.Select(x => x.ErrorMessage)
        };
    }
}