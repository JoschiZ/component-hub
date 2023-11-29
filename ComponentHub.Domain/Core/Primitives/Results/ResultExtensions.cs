using FluentValidation;
using FluentValidation.Results;

namespace ComponentHub.Domain.Core.Primitives.Results;

public static class ResultExtensions
{
    public static ResultValidation<TResult> Validate<TResult>(this ResultValidation<TResult> result, IValidator<TResult> validator)
    {
        return result.IsSuccess ? validator.Validate(result.ResultObject).Errors : [];
    }
}