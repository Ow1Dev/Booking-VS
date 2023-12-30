/*namespace Core.ResultTypes;

public interface IValidationResult
{
    public static readonly Error ValidationErrror = new(
        "ValidationError",
        "A validation problem has occurred.");
    
    Error[] Errors { get; }
}

public class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] errors) 
        : base( false, IValidationResult.ValidationErrror)
    {
        Errors = errors;
    }
    
    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}

public class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] errors) 
        : base( default, false, IValidationResult.ValidationErrror)
    {
        Errors = errors;
    }
    
    public Error[] Errors { get; }

    public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
}*/