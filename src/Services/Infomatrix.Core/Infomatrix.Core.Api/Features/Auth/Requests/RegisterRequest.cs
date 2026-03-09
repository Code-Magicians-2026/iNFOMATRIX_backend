namespace Infomatrix.Core.Api.Features.Auth.Requests;

public record RegisterRequest(
    string FullName,
    string Email,
    string Password);
