namespace Infomatrix.Core.Api.Dtos.Auth;

public record RegisterRequest(
    string Email,
    string Password);
