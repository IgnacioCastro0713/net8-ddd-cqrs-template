namespace Domain.Common;

public static class ResultState
{
	public const bool Failure = false;
	public const bool Success = true;
}

public readonly struct Result
{
	public bool IsSuccess { get; }

	public bool IsFailure => !IsSuccess;

	public Error Error { get; }

	public Result()
	{
		IsSuccess = ResultState.Success;
		Error = Error.None;
	}

	private Result(Error error)
	{
		IsSuccess = ResultState.Failure;
		Error = error;
	}

	public static Result Failure(Error error) => new(error);

	public static Result Success() => new();
	
	public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)
	{
		return IsSuccess
			? onSuccess()
			: onFailure(Error);
	}
}

public readonly struct Result<T>
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

	public static implicit operator Result<T>(Result result) => new(result.Error);

	public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
	{
		return IsSuccess
			? onSuccess(Value)
			: onFailure(Error);
	}
}