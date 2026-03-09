using Infomatrix.Core.Domain.Features.Auth;
using Infomatrix.Core.Infrastructure.Auth;
using Infomatrix.Core.Shared;
using Microsoft.Extensions.Logging;
using Supabase.Gotrue.Exceptions;
using System.Text.Json;

namespace Infomatrix.Core.Infrastructure.Extensions.Supabase;

public static class GotrueExceptionExtensions
{
    public static Result HandleGoTrueException(
        this GotrueException ex,
        ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(ex.Message))
        {
            logger.LogError(ex, "Supabase GoTrueException without message");
            return Result.Failure(AuthErrors.UnknownError);
        }

        if (!TryParseSupabaseError(ex.Message, out var error))
        {
            logger.LogWarning(
                "Supabase non-JSON error: {Message}",
                ex.Message);
        
            return Result.Failure(FromResponseMessage(ex.Message));
        }


        logger.LogWarning("Supabase error: Code={Code}, ErrorCode={ErrorCode} - {Message}",
            error.Code,
            error.ErrorCode,
            error.Message);

        return Result.Failure(error.ToError());
    }

    private static bool TryParseSupabaseError(
        string message,
        out SupabaseError error)
    {
        error = null!;

        try
        {
            error = JsonSerializer
                .Deserialize<SupabaseError>(message)!;

            return error is not null;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static Error FromResponseMessage(string response)
    {
        response = response.ToLowerInvariant();

        if (response.Contains("upstream request timeout"))
        {
            return AuthErrors.Unavailable;
        }

        return AuthErrors.UnknownError;
    }
}
