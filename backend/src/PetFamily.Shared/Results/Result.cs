using PetFamily.Shared.Errors;
using PetFamily.Shared.Results.Interfaces;

namespace PetFamily.Shared.Results;

public class Result : IResult
{
    public bool IsSuccess => Error == null;

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    protected Result()
    {
        Error = default;
    }

    protected Result(Error error)
    {
        Error = error;
    }

    public static Result Success() => new();

    public static Result Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new Result(error);
    }

    public static implicit operator Result(Error error) => Failure(error);
}

public class Result<TValue> : Result
{
    private readonly TValue _value = default!;

    private Result(TValue value)
    {
        _value = value;
    }

    private Result(Error error) : base(error)
    {
    }

    public new static Result<TValue> Success(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Result<TValue>(value);
    }

    public new static Result<TValue> Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new Result<TValue>(error);
    }

    public TValue? Value =>
        IsSuccess
            ? _value
            : throw new InvalidOperationException("Can't access value when result is failure.");

    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(Error error) => Failure(error);
}