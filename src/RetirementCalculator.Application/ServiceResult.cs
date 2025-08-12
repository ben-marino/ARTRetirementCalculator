namespace RetirementCalculator.Application;

public class ServiceResult<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }

    private ServiceResult() { }

    public static ServiceResult<T> Success(T value)
    {
        return new ServiceResult<T>
        {
            IsSuccess = true,
            Value = value
        };
    }

    public static ServiceResult<T> Failure(string error)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            Error = error
        };
    }
}