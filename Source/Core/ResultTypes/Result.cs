namespace Core.ResultTypes;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error[] errors) 
        : base(isSuccess, errors) =>
        _value = value;
    
    protected internal Result(TValue? value, bool isSuccess, Error error) 
        : base(isSuccess, error) =>
        _value = value;

    public TValue Value => IsSuccess 
        ? _value! 
        : throw new InvalidOperationException("The value can not be access on a failed");

    public static implicit operator Result<TValue>(TValue value) => Create(value);
}

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }
        
        IsSuccess = isSuccess;
        Errors = new[] { error };
    }

    protected internal Result(bool isSuccess, Error[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }
    
    public bool IsFailure => !IsSuccess;

    public Error[] Errors { get; }

    public static Result Success() => new(true, Error.None);
    public static Result<TValue> Success<TValue>(TValue value) => 
        new(value, true, Error.None);
    
    public static Result Failure(Error error) => 
        new(false, error);
    
    public static Result Failure(Error[] errors) => 
        new(false, errors);
    public static Result<TValue> Failure<TValue>(Error error) => 
        new(default,false, error);
    
    public static Result<TValue> Failure<TValue>(Error[] errors) => 
        new(default,false, errors);

    public static Result<TValue> Create<TValue>(TValue? value) 
        => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static Result<T> Ensure<T>(T value, Func<T, bool> predicate, Error error)
    {
        return predicate(value) ? Success(value) : Failure<T>(error);
    }

    public static Result<T> Ensure<T>(
        T value,
        params (Func<T, bool> predicate, Error error)[] functions)
    {
        var results = new List<Result<T>>();
        foreach (var (predicate, error) in functions)
        {
            results.Add(Ensure(value, predicate, error));
        }

        return Combine(results.ToArray());
    }

    private static Result<T> Combine<T>(params Result<T>[] results)
    {
        if (results.Any(r => r.IsFailure))
        {
            return Failure<T>(
                results.SelectMany(r => r.Errors)
                    .Where(e => e != Error.None)
                    .Distinct()
                    .ToArray());
        }

        return Success(results[0].Value);
    }

    private static Result<(T1, T2)> Combine<T1, T2>(Result<T1> results1, Result<T2> results2)
    {
        if (results1.IsFailure)
        {
            return Failure<(T1, T2)>(results1.Errors);
        }
        
        if (results2.IsFailure)
        {
            return Failure<(T1, T2)>(results2.Errors);
        }

        return Success((results1.Value, results2.Value));
    }

}