using Infomatrix.Core.Domain.Features.Auth;
using Infomatrix.Core.Infrastructure.Auth;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Infrastructure.Extensions.Supabase;

public static class SupabaseErrorExtensions
{
    public static Error ToError(this SupabaseError supabaseError)
    {
        return supabaseError.ErrorCode switch
        {
            "invalid_credentials" => AuthErrors.InvalidCredentials,
            "otp_expired" => AuthErrors.OtpExpired,
            "session_expired" => AuthErrors.SessionExpired,
            "validation_failed" => AuthErrors.ValidationFailed(supabaseError.Message),
            "over_email_send_rate_limit" => AuthErrors.RateLimit(supabaseError.Message),
            "unexpected_failure" => AuthErrors.Unavailable,
            _ => Error.BadRequest(supabaseError.ErrorCode, supabaseError.Message)
        };
    }
}
