namespace Domain.Shared;

public interface IResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
}

public static class ResultState
{
    public const bool Failure = false;
    public const bool Success = true;
}

public class Result<T> : IResult
{
    public readonly T Value;
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    private Result(T value)
    {
        IsSuccess = ResultState.Success;
        Error = Error.None;
        Value = value;
    }

    private Result(Error error)
    {
        IsSuccess = ResultState.Failure;
        Error = error;
        Value = default!;
    }

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(Error error) => new(error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess
            ? onSuccess(Value)
            : onFailure(Error);
}