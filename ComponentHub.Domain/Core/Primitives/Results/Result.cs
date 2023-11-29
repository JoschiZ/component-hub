using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ComponentHub.Domain.Core.Primitives.Results;

/// <summary>
/// Base result class with generic TResult and TError 
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <typeparam name="TError"></typeparam>
public class Result<TResult, TError>
{
    [JsonConstructor]
    protected Result(bool isSuccess, TResult? resultObject, TError? error)
    {
        IsSuccess = isSuccess;
        ResultObject = resultObject;
        Error = error;
    }
    


    public static Result<TResult, TError> CreateError(TError error)
    {
        return new Result<TResult, TError>(
            false,
            default,
            error);
    }

    public static Result<TResult, TError> CreateSuccess(TResult result)
    {
        return new Result<TResult, TError>(true, result, default);
    }

    /// <summary>
    /// Applies a action to the contained object
    /// </summary>
    /// <param name="action"></param>
    /// <typeparam name="TReturn"></typeparam>
    /// <returns></returns>
    public Result<TReturn, TError> Bind<TReturn>(Func<TResult, TReturn> action)
    {
        return IsSuccess ? action(ResultObject) : Error;
    }
    
    /// <summary>
    /// Explicitly handles both the error and success state
    /// </summary>
    /// <param name="successPath"></param>
    /// <param name="errorPath"></param>
    /// <returns></returns>
    public TReturn Match<TReturn>(
        Func<TResult, TReturn> successPath,
        Func<TError, TReturn> errorPath)
    {
        return IsSuccess ? successPath(ResultObject) : errorPath(Error);
    }

    public TReturn Match<TReturn>(TReturn success, TReturn error) => IsSuccess ? success : error;

    [MemberNotNullWhen(returnValue: true, nameof(ResultObject))]
    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public bool IsSuccess { get; }

    
    [MemberNotNullWhen(returnValue: true, nameof(Error))]
    [MemberNotNullWhen(returnValue: false, nameof(ResultObject))]
    public bool IsError => !IsSuccess;

    public TResult? ResultObject { get; }
    
    public TError? Error { get; }

    public static implicit operator Result<TResult, TError>(TError error) => CreateError(error);

    public static implicit operator Result<TResult, TError>(TResult result) => CreateSuccess(result);
}
