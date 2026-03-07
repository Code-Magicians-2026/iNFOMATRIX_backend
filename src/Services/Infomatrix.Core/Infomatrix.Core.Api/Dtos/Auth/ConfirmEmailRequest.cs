namespace Infomatrix.Core.Api.Dtos.Auth;

public record ConfirmEmailRequest(
    string Email,
    string Token,
    string Password);
