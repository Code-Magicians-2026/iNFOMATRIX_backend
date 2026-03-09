namespace Infomatrix.Core.Api.Features.Auth.Responses;

public record TokenResponse(
    string AccessToken,
    long ExpiresIn,
    string RefreshToken,
    Guid UserId,
    string? Email);
