namespace Infomatrix.Core.Api.Features.Auth.Requests;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);
