using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ComponentHub.Shared.Results;

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
