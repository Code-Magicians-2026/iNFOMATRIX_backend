using Infomatrix.Core.Api.Base;
using Infomatrix.Core.Api.Extensions;
using Infomatrix.Core.Api.Features.Auth.Mappers;
using Infomatrix.Core.Api.Features.Auth.Requests;
using Infomatrix.Core.Api.Features.Auth.Responses;
using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Application.Features.Auth.CheckEmail;
using Infomatrix.Core.Application.Features.Auth.ConfirmEmail;
using Infomatrix.Core.Application.Features.Auth.Login;
using Infomatrix.Core.Application.Features.Auth.RefreshToken;
using Infomatrix.Core.Application.Features.Auth.Register;
using Infomatrix.Core.Application.Features.Auth.RequestResetPassword;
using Infomatrix.Core.Application.Features.Auth.ResetPassword;
using Infomatrix.Core.Application.Features.Auth.VerifyOtp;
using Infomatrix.Core.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Infomatrix.Core.Api.Features.Auth;

[Route("api/auth")]
[ApiController]
public class AuthController : BaseController
{
    public AuthController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost("check-email")]
    [ProducesResponseType<EmailResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckEmailAsync(
        [FromBody] EmailRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CheckEmailCommand(request.Email);

        var result = await _sender
            .Send(command, cancellationToken);

        return result
            .Map(r => r.ToResponse())
            .ToActionResult();
    }

    [HttpPost("register")]
    [ProducesResponseType<EmailResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(
            request.FullName,
            request.Email,
            request.Password);

        var result = await _sender
            .Send(command, cancellationToken);

        return result
            .Map(r => r.ToResponse())
            .ToActionResult();
    }

    [HttpPost("confirm-email")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmEmailAsync(
        [FromBody] ConfirmEmailRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ConfirmEmailCommand(
            request.Email,
            request.Token);

        var result = await _sender
            .Send(command, cancellationToken);

        return result
            .Map(r => r.ToResponse())
            .ToActionResult();
    }

    [HttpPost("login")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.Email,
            request.Password);

        var result = await _sender
            .Send(command, cancellationToken);

        return result
            .Map(r => r.ToResponse())
            .ToActionResult();
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshTokenAsync(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(
            request.AccessToken,
            request.RefreshToken);

        var result = await _sender
            .Send(command, cancellationToken);

        return result
            .Map(r => r.ToResponse())
            .ToActionResult();
    }

    [HttpPost("request-reset-password")]
    [ProducesResponseType<EmailResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RequestResetPasswordAsync(
        [FromBody] RequestResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RequestResetPasswordCommand(
            request.Email);

        var result = await _sender
            .Send(command, cancellationToken);

        return result
            .Map(r => r.ToResponse())
            .ToActionResult();
    }

    [HttpPost("verify-otp")]
    [ProducesResponseType<EmailResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyOtpAsync(
        [FromBody] VerifyOtpRequest request,
        CancellationToken cancellationToken)
    {
        var command = new VerifyOtpCommand(
            request.Email,
            request.Token);

        var result = await _sender
            .Send(command, cancellationToken);

        return result
            .Map(r => r.ToResponse())
            .ToActionResult();
    }

    [HttpPost("reset-password")]
    [ProducesResponseType<TokenDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPasswordAsync(
        [FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ResetPasswordCommand(
            request.Email,
            request.NewPassword);

        var result = await _sender
            .Send(command, cancellationToken);

        return result
            .Map(r => r.ToResponse())
            .ToActionResult();
    }
}
