namespace Infomatrix.Core.Api.Dtos;

public record TokenDto(
    string AccessToken,
    string RefreshToken,
    long ExpiresIn,
    string TokenType);
