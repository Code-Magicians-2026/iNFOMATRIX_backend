namespace Infomatrix.Core.Application.Exceptions;

public class AuthenticationException : Exception
{
    public AuthenticationException(string message)
        : base(message)
    {
    }

    public static void ThrowInvalidCredentials()
        => throw new AuthenticationException("Invalid email or password.");

    public static void ThrowUserNotFound(string identifier)
        => throw new AuthenticationException($"User not found: {identifier}.");

    public static void ThrowEmailAlreadyExists(string email)
        => throw new AuthenticationException($"User with email '{email}' already exists.");

    public static void ThrowTokenExpired()
        => throw new AuthenticationException("Token has expired.");

    public static void ThrowInvalidToken()
        => throw new AuthenticationException("Invalid or malformed token.");

    public static void ThrowNullSession()
        => throw new AuthenticationException("Session could not be established.");

    public static void ThrowUnknownError(string? details = null)
        => throw new AuthenticationException(
            details is null ? "An unknown authentication error occurred." : $"An unknown authentication error occurred: {details}");

    public static void ThrowOAuthProviderError(string provider, string? details = null)
        => throw new AuthenticationException(
            details is null ? $"Failed to authenticate with {provider}." : $"Failed to authenticate with {provider}: {details}");
}
