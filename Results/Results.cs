namespace HotelListing.Api.Results;

public readonly record struct Error(string Code, string Description)
{
    public static readonly Error None = new("", "");
    public bool IsNone => string.IsNullOrWhiteSpace(Code);
}

public readonly record struct Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public Error[] Errors { get; init; } = Array.Empty<Error>();

    private Result(bool isSuccess, T? value, Error[] errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public static Result<T> Success(T value) => new(true, value, Array.Empty<Error>());
    public static Result<T> Failure(params Error[] errors) => new(false, default, errors);
    public static Result<T> NotFound(Error error) => new(false, default, [error]);

    // Functional helpers
    public Result<K> Map<K>(Func<T, K> mapFunc)
    {
        if (IsSuccess && Value is not null)
        {
            return Result<K>.Success(mapFunc(Value));
        }
        else
        {
            return Result<K>.Failure(Errors);
        }
    }

    public Result<T> Bind(Func<T, Result<T>> bindFunc)
    {
        if (IsSuccess && Value is not null)
        {
            return bindFunc(Value);
        }
        else
        {
            return Result<T>.Failure(Errors);
        }
    }
}
