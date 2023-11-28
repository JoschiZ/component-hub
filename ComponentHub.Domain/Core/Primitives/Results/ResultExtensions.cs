namespace ComponentHub.Domain.Core.Primitives.Results;

public static class ResultExtensions
{
    public static TResult MapResult<TResult, TError>(this Result<TResult, TError> result, Func<TError, TResult> failure)
    {
        return result.IsSuccess ? result.ResultObject : failure(result.Error);
    }
}