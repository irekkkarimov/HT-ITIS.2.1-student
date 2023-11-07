namespace Hw8.Utils;

public struct Result<T>
{
    public ResultStatus Status { get; init; }
    public T Value { get; init; }
    public string Message { get; init; }

    public static Result<T> Success(T value)
    {
        return new Result<T>
        {
            Status = ResultStatus.Success,
            Value = value,
            Message = "Success"
        };
    }
    
    public static Result<double> Failure(string errorMessage)
    {
        return new Result<double>
        {
            Status = ResultStatus.Failure,
            Message = errorMessage
        };
    }
}

public enum ResultStatus
{
    Success,
    Failure
}