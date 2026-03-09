using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Domain.Features.Auth;
using Infomatrix.Core.Shared;
using Microsoft.Extensions.Logging;
using Supabase.Gotrue;

namespace Infomatrix.Core.Infrastructure.Extensions.Supabase;

public static class SessionExtensions
{
    public static Result<TokenDto> HandleSession(
        this Session session,
        ILogger logger)
    {
        if (session is null)
        {
            return Result.Failure<TokenDto>(AuthErrors.UnknownError);
        }

        if (!Guid.TryParse(session.User!.Id, out var userId))
        {
            logger.LogError("Invalid user Id format: {UserId}", session.User.Id);
            return Result.Failure<TokenDto>(AuthErrors.UnknownError);
        }

        var tokenDto = new TokenDto(
            session.AccessToken!,
            session.ExpiresIn,
            session.RefreshToken!,
            userId,
            session.User.Email
        );

        return Result.Success(tokenDto);
    }
}
