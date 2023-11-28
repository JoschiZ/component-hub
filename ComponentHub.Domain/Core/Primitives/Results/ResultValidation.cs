using System.Text.Json.Serialization;
using FluentValidation.Results;

namespace ComponentHub.Domain.Core.Primitives.Results;

/// <summary>
/// Convenience Shortcut for <see cref="Result{TResult, TError}"/> with a List{ValidationFailure} as error type
/// </summary>
/// <typeparam name="TResult"></typeparam>
public sealed class ResultValidation<TResult>: Result<TResult, List<ValidationFailure>>
{
    [JsonConstructor]
    private ResultValidation(bool isSuccess, TResult? resultObject, List<ValidationFailure>? error) : base(isSuccess, resultObject, error)
    {
    }
    
    public new ResultValidation<TReturn> Bind<TReturn>(Func<TResult, TReturn> action)
    {
        return IsSuccess ? action(ResultObject) : Error;
    }


    public new static ResultValidation<TResult> CreateError(List<ValidationFailure> validationResults)
    {
        return new ResultValidation<TResult>(false, default, validationResults);
    }
    
    public new static ResultValidation<TResult> CreateSuccess(TResult result)
    {
        return new ResultValidation<TResult>(true, result, default);
    }
    
    public static implicit operator ResultValidation<TResult>(List<ValidationFailure> errors) => CreateError(errors);
    public static implicit operator ResultValidation<TResult>(TResult result) => CreateSuccess(result);
}