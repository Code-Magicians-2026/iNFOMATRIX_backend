namespace Infomatrix.Core.Api.Dtos.Auth;

public record RefreshTokenRequest(
    string AccessToken,
    string RefreshToken);
