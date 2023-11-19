namespace ComponentHub.Shared.Results;

public class ResultError<TResult>: Result<TResult, Error>
{
    protected ResultError(bool isSuccess, TResult? resultObject, Error? error) : base(isSuccess, resultObject, error)
    {
    }
    
    public new static ResultError<TResult> CreateError(Error validationResults)
    {
        return new ResultError<TResult>(false, default, validationResults);
    }
    
    public new static ResultError<TResult> CreateSuccess(TResult result)
    {
        return new ResultError<TResult>(true, result, default);
    }
    
    public static implicit operator ResultError<TResult>(Error errors) => CreateError(errors);
    public static implicit operator ResultError<TResult>(TResult result) => CreateSuccess(result);
}