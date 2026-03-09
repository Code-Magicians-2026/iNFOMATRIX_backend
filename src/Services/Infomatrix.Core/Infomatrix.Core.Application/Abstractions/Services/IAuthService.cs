using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Abstractions.Services;

public interface IAuthService
{
    Task<Result<string>> RegisterUserAsync(string email, string password);

    Task<Result<TokenDto>> ConfirmEmailAsync(string email, string token, string lastPassword);

    Task<Result<TokenDto>> LoginAsync(string email, string password);

    Task<Result<TokenDto>> LoginWithGoogleAsync(string idToken);

    Task<Result<TokenDto>> LoginWithAppleAsync(string idToken);

    Task<Result<TokenDto>> RefreshTokenAsync(string accessToken, string refreshToken);

    Task<Result<string>> RequestResetPasswordAsync(string email);

    Task<Result<TokenDto>> VerifyOtpAsync(string email, string token);

    Task<Result<TokenDto>> ResetPasswordAsync(string email, string newPassword, TokenDto tokenDto);

    Task<Result> DeleteAccountAsync(string id);
}
