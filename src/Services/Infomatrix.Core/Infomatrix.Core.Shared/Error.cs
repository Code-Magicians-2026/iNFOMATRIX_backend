namespace Infomatrix.Core.Shared;

public record Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new("Error.NullValue", "Value is null.", ErrorType.Failure);

    private Error(string Code, string Message, ErrorType errorType)
    {
        this.Code = Code;
        this.Message = Message;
        Type = errorType;
    }

    public string Code { get; }

    public string Message { get; }

    public ErrorType Type { get; }

    public static Error BadRequest(string code, string message) =>
        new(code, message, ErrorType.BadRequest);

    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static Error Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);

    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);

    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);

    public static Error Unauthorized(string code, string message) =>
        new(code, message, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string message) =>
        new(code, message, ErrorType.Forbidden);

    public static Error TooManyRequests(string code, string message) =>
        new(code, message, ErrorType.TooManyRequests);

    public static Error Unavailable(string code, string message) =>
        new(code, message, ErrorType.Unavailable);

    public override string ToString()
    {
        return $"{Code}. {Message}";
    }
}
