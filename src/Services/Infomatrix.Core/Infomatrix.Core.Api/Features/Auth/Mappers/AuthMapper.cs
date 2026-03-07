using Infomatrix.Core.Api.Features.Auth.Responses;
using Infomatrix.Core.Application.DTOs.Auth;

namespace Infomatrix.Core.Api.Features.Auth.Mappers;

public static class AuthMapper
{
    public static TokenResponse ToResponse(this TokenDto dto)
    {
        return new TokenResponse(
            dto.AccessToken,
            dto.RefreshToken,
            (int)dto.ExpiresIn,
            dto.TokenType);
    }

    public static EmailResponse ToResponse(this string email)
    {
        return new EmailResponse(email);
    }
}
