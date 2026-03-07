namespace Infomatrix.Core.Application.DTOs.Auth;

public record TokenDto(
    string AccessToken,
    string RefreshToken,
    long ExpiresIn,
    string TokenType);
