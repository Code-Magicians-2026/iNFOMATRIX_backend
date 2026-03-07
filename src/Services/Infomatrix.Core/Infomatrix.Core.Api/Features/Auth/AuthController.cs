using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.DTOs;
using Infomatrix.Core.Api.Features.Auth.Requests;
using Infomatrix.Core.Api.Features.Auth.Responses;
using Infomatrix.Core.Api.Features.Auth.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Infomatrix.Core.Api.Features.Auth;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType<EmailResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterRequest request)
    {
        var email = await _authService.RegisterUserAsync(
            request.Email,
            request.Password);

        return Ok(email.ToResponse());
    }

    [HttpPost("confirm-email")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmEmailAsync(
        [FromBody] ConfirmEmailRequest request)
    {
        var tokenDto = await _authService.ConfirmEmailAsync(
            request.Email,
            request.Token,
            request.Password);

        return Ok(tokenDto.ToResponse());
    }

    [HttpPost("login")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginRequest request)
    {
        var tokenDto = await _authService.LoginAsync(
            request.Email,
            request.Password);

        return Ok(tokenDto.ToResponse());
    }

    [HttpPost("google")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginWithGoogleAsync(
        [FromBody] IdTokenRequest request)
    {
        var tokenDto = await _authService.LoginWithGoogleAsync(request.IdToken);

        return Ok(tokenDto.ToResponse());
    }

    [HttpPost("apple")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginWithAppleAsync(
        [FromBody] IdTokenRequest request)
    {
        var tokenDto = await _authService.LoginWithAppleAsync(request.IdToken);

        return Ok(tokenDto.ToResponse());
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshTokenAsync(
        [FromBody] RefreshTokenRequest request)
    {
        var tokenDto = await _authService.RefreshTokenAsync(
            request.AccessToken,
            request.RefreshToken);

        return Ok(tokenDto.ToResponse());
    }

    [HttpPost("request-reset-password")]
    [ProducesResponseType<EmailResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RequestResetPasswordAsync(
        [FromBody] RequestResetPasswordRequest request)
    {
        var email = await _authService.RequestResetPasswordAsync(request.Email);

        return Ok(email.ToResponse());
    }

    [HttpPost("verify-otp")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyOtpAsync(
        [FromBody] VerifyOtpRequest request)
    {
        var tokenDto = await _authService.VerifyOtpAsync(
            request.Email,
            request.Token);

        return Ok(tokenDto.ToResponse());
    }

    [HttpPost("reset-password")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPasswordAsync(
        [FromBody] ResetPasswordRequest request)
    {
        var inputToken = new TokenDto(
            request.AccessToken,
            request.RefreshToken,
            0,
            "bearer");

        var tokenDto = await _authService.ResetPasswordAsync(
            request.Email,
            request.NewPassword,
            inputToken);

        return Ok(tokenDto.ToResponse());
    }
}
