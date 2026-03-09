using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Domain.Features.Auth;

public static class AuthErrors
{
    public static Error AlreadyExists(string email) => Error.Conflict(
        "Auth.UserAlreadyExists",
        $"User with email = '{email}' already exists");

    public static readonly Error NullSession = Error.Failure(
        "Auth.NullSession",
        "Session is null");

    public static readonly Error UnknownError = Error.Failure(
        "Auth.UnknownError",
        "An unknown error occurred");

    public static readonly Error TokenExpired = Error.Unauthorized(
        "Auth.TokenExpired",
        "Confirmation token and cached password expire after 10 minutes. Please try registering again");

    public static readonly Error InvalidCredentials = Error.Unauthorized(
        "Auth.InvalidCredentials",
        "Invalid credentials provided.");

    public static readonly Error OtpExpired = Error.BadRequest(
        "Auth.OtpExpired",
        "The one-time password has expired.");

    public static readonly Error SessionExpired = Error.Unauthorized(
        "Auth.SessionExpired",
        "The session has expired. Please log in again.");

    public static Error ValidationFailed(string message) => Error.Validation(
        "Auth.ValidationFailed",
        message);

    public static Error RateLimit(string message) => Error.TooManyRequests(
        "Auth.TooManyRequests",
        message);

    public static Error UserNotFound(string id) => Error.NotFound(
        "Auth.UserNotFound",
        $"User with id = '{id}' was not found");

    public static Error Unavailable => Error.Unavailable(
        "Auth.ServiceUnavailable",
        "Authentication service is temporarily unavailable. Please try again later.");
}
