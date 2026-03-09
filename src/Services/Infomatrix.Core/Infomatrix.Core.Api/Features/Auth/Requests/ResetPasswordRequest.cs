namespace Infomatrix.Core.Api.Features.Auth.Requests;

public record ResetPasswordRequest(
    string Email,
    string NewPassword);
