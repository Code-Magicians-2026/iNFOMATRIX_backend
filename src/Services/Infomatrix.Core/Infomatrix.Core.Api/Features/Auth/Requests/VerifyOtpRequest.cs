namespace Infomatrix.Core.Api.Features.Auth.Requests;

public record VerifyOtpRequest(
    string Email,
    string Token);
