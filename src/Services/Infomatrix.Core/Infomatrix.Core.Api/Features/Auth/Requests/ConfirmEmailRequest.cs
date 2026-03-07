namespace Infomatrix.Core.Api.Features.Auth.Requests;

public record ConfirmEmailRequest(
    string Email,
    string Token,
    string Password);
