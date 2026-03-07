namespace Infomatrix.Core.Api.Features.Auth.Responses;

public record TokenResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    string TokenType);
