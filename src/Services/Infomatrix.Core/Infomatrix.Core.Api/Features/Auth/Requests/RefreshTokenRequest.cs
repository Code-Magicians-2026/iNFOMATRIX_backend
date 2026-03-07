namespace Infomatrix.Core.Api.Features.Auth.Requests;

public record RefreshTokenRequest(
    string AccessToken,
    string RefreshToken);
