namespace Infomatrix.Core.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationError = Error.Validation(
        "ValidationError",
        "A validation problem occurred.");

    Error[] Errors { get; }
}
