namespace Infomatrix.Core.Application.DTOs.Auth;

public record TokenDto(
    string AccessToken,
    long ExpiresIn,
    string RefreshToken,
    Guid UserId,
    string? Email);
