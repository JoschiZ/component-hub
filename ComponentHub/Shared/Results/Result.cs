using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ComponentHub.Shared.Results;

public sealed class Result<TResult>
{
    [JsonConstructor]
    private Result(bool isSuccess, TResult? resultObject, Error? error)
    {
        IsSuccess = isSuccess;
        ResultObject = resultObject;
        Error = error;
    }

    public static Result<TResult> CreateError(Error error)
    {
        return new Result<TResult>(
            false,
            default,
            error);
    }

    public static Result<TResult> CreateSuccess(TResult result)
    {
        return new Result<TResult>(true, result, null);
    }

    [MemberNotNullWhen(returnValue: true, nameof(ResultObject))]
    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public bool IsSuccess { get; }

    
    [MemberNotNullWhen(returnValue: true, nameof(Error))]
    [MemberNotNullWhen(returnValue: false, nameof(ResultObject))]
    public bool IsError => !IsSuccess;

    public TResult? ResultObject { get; }
    
    public Error? Error { get; }

    public static implicit operator Result<TResult>(Error error) => CreateError(error);

    public static implicit operator Result<TResult>(TResult result) => CreateSuccess(result);
}

