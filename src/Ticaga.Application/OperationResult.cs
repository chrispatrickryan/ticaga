using Ticaga.Application.Common;

namespace Ticaga.Application;

public sealed class OperationResult<T>
{
    public bool IsSuccess { get; init; }

    public T? Value { get; init; }

    public TicagaErrorType? ErrorType { get; init; }

    public string? ErrorMessage { get; init; }

    public Dictionary<string, string[]>? ValidationErrors { get; init; }

    private OperationResult(
        bool isSuccess,
        T? value,
        TicagaErrorType? errorType,
        string? errorMessage,
        Dictionary<string, string[]>? validationErrors)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorType = errorType;
        ErrorMessage = errorMessage;
        ValidationErrors = validationErrors;
    }

    public static OperationResult<T> Success(T value) 
        => new(true, value, null, null, null);

    public static OperationResult<T> ValidationProblem(Dictionary<string, string[]> errors) 
        => new(false, default, TicagaErrorType.Validation, "One or more validation errors occurred.", errors);

    public static OperationResult<T> NotFound(string message) 
        => new(false, default, TicagaErrorType.NotFound, message, null);

    public static OperationResult<T> Conflict(string message) 
        => new(false, default, TicagaErrorType.Conflict, message, null);

    public static OperationResult<T> Unauthorized(string message) 
        => new(false, default, TicagaErrorType.Unauthorized, message, null);
}
