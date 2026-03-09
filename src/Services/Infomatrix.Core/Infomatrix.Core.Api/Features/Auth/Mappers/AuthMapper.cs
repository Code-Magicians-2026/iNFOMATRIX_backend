using Infomatrix.Core.Api.Features.Auth.Responses;
using Infomatrix.Core.Application.DTOs.Auth;

namespace Infomatrix.Core.Api.Features.Auth.Mappers;

public static class AuthMapper
{
    public static TokenResponse ToResponse(this TokenDto dto)
    {
        return new TokenResponse(
            dto.AccessToken,
            dto.ExpiresIn,
            dto.RefreshToken,
            dto.UserId,
            dto.Email
            );
    }

    public static EmailResponse ToResponse(this EmailDto dto)
    {
        return new EmailResponse(
            dto.Email);
    }
}
