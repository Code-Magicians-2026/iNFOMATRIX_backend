namespace Infomatrix.Core.Api.Features.Auth.Requests;

public record RegisterRequest(
    string Email,
    string Password);
