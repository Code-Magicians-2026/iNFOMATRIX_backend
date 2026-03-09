using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Application.Exceptions;
using Infomatrix.Core.Domain.Features.Auth;
using Infomatrix.Core.Infrastructure.Extensions.Supabase;
using Infomatrix.Core.Shared;
using Infomatrix.Core.Shared.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
using System.Text.Json;
using SupabaseClient = Supabase.Client;

namespace Infomatrix.Core.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly string _secretKey;
    private readonly SupabaseClient _supabaseClient;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        SupabaseClient supabaseClient,
        ILogger<AuthService> logger,
        IOptionsSnapshot<SupabaseOptions> options)
    {
        _supabaseClient = supabaseClient;
        _logger = logger;
        _secretKey = options.Value.AdminKey;
    }

    public async Task<Result<string>> RegisterUserAsync(string email, string password)
    {
        try
        {
            var session = await _supabaseClient.Auth.SignUp(email, password);

            if (session?.User is null)
                return Result.Failure<string>(AuthErrors.UnknownError);

            if (session.User.IsFakeUser())
                return Result.Failure<string>(AuthErrors.AlreadyExists(email));

            return Result.Success(email);
        }
        catch (GotrueException ex)
        {
            return ex.HandleGoTrueException(_logger)
                .MapFailure<string>();
        }
    }

    public async Task<Result<TokenDto>> ConfirmEmailAsync(
        string email,
        string token,
        string lastPassword)
    {
        if (string.IsNullOrEmpty(lastPassword))
        {
            return Result.Failure<TokenDto>(AuthErrors.TokenExpired);
        }

        try
        {
            var session = await _supabaseClient.Auth
                .VerifyOTP(email, token, Constants.EmailOtpType.Email);

            if (session is null)
            {
                return HandleNullSession<TokenDto>(
                    nameof(ConfirmEmailAsync),
                    $"Email: {email}");
            }

            await TryUpdatePasswordAsync(lastPassword);

            return session.HandleSession(_logger);
        }
        catch (GotrueException ex)
        {
            return ex.HandleGoTrueException(_logger)
                .MapFailure<TokenDto>();
        }
    }

    public async Task<Result<TokenDto>> LoginAsync(string email, string password)
    {
        try
        {
            var session = await _supabaseClient.Auth
                .SignIn(email, password);

            if (session is null)
            {
                return HandleNullSession<TokenDto>(
                    nameof(LoginAsync),
                    $"Email: {email}");
            }

            return session.HandleSession(_logger);
        }
        catch (GotrueException ex)
        {
            return ex.HandleGoTrueException(_logger)
                .MapFailure<TokenDto>();
        }
    }

    public Task<Result<TokenDto>> LoginWithGoogleAsync(string idToken) =>
        LoginWithProviderAsync(idToken, Constants.Provider.Google);

    public Task<Result<TokenDto>> LoginWithAppleAsync(string idToken) =>
        LoginWithProviderAsync(idToken, Constants.Provider.Apple);

    private async Task<Result<TokenDto>> LoginWithProviderAsync(
        string idToken,
        Constants.Provider provider)
    {
        try
        {
            var session = await _supabaseClient.Auth.SignInWithIdToken(
                provider: provider,
                idToken: idToken);

            if (session is null)
            {
                return HandleNullSession<TokenDto>(
                    nameof(LoginWithProviderAsync),
                    $"Provider: {provider}");
            }

            return session.HandleSession(_logger);
        }
        catch (GotrueException ex)
        {
            return ex.HandleGoTrueException(_logger)
                .MapFailure<TokenDto>();
        }
    }

    public async Task<Result<TokenDto>> RefreshTokenAsync(
        string accessToken,
        string refreshToken)
    {
        try
        {
            var session = await _supabaseClient.Auth
                .SetSession(accessToken, refreshToken, false);

            session = await _supabaseClient.Auth
                .RefreshSession();

            if (session is null)
            {
                return HandleNullSession<TokenDto>(
                    nameof(RefreshTokenAsync));
            }

            return session.HandleSession(_logger);
        }
        catch (GotrueException ex)
        {
            return ex.HandleGoTrueException(_logger)
                .MapFailure<TokenDto>();
        }
    }

    public async Task<Result<string>> RequestResetPasswordAsync(string email)
    {
        try
        {
            await _supabaseClient.Auth
                .ResetPasswordForEmail(email);

            return Result.Success(email);
        }
        catch (GotrueException ex)
        {
            return ex.HandleGoTrueException(_logger)
                .MapFailure<string>();
        }
    }

    public async Task<Result<TokenDto>> VerifyOtpAsync(string email, string token)
    {
        try
        {
            var session = await _supabaseClient.Auth.VerifyOTP(
                email,
                token,
                Constants.EmailOtpType.Recovery);

            if (session is null)
            {
                return HandleNullSession<TokenDto>(
                    nameof(VerifyOtpAsync),
                    $"Email: {email}");
            }

            return session.HandleSession(_logger);
        }
        catch (GotrueException ex)
        {
            return ex.HandleGoTrueException(_logger)
                .MapFailure<TokenDto>();
        }
    }

    public async Task<Result<TokenDto>> ResetPasswordAsync(
        string email,
        string newPassword,
        TokenDto tokenDto)
    {
        try
        {
            var session = await _supabaseClient.Auth.SetSession(
                tokenDto.AccessToken,
                tokenDto.RefreshToken);

            if (session is null)
            {
                return HandleNullSession<TokenDto>(
                    nameof(ResetPasswordAsync),
                    $"Email: {email}");
            }

            await TryUpdatePasswordAsync(newPassword);

            return session.HandleSession(_logger);
        }
        catch (GotrueException ex)
        {
            return ex.HandleGoTrueException(_logger)
                .MapFailure<TokenDto>();
        }
    }

    public async Task<Result> DeleteAccountAsync(string id)
    {
        try
        {
            var adminAuthClient = _supabaseClient
                .AdminAuth(_secretKey);

            var user = await adminAuthClient
                .GetUserById(id);

            if (user is null)
            {
                return Result.Failure(
                    AuthErrors.UserNotFound(id));
            }

            await adminAuthClient
                .DeleteUser(id);

            return Result.Success();
        }
        catch (GotrueException ex)
        {
            return ex.HandleGoTrueException(_logger);
        }
    }

    private async Task TryUpdatePasswordAsync(string password)
    {
        try
        {
            await _supabaseClient.Auth.Update(new UserAttributes
            {
                Password = password
            });
        }
        catch (GotrueException ex)
        {
            var error = JsonSerializer
                .Deserialize<SupabaseError>(ex.Message);

            if (error == null || !error.ErrorCode.Contains("same_password"))
            {
                throw;
            }
        }
    }

    private Result<T> HandleNullSession<T>(string context, string? details = default)
    {
        _logger.LogWarning(
            "Session is null. Context: {Context}, Details: {Details}",
            context,
            details ?? "Without any details");

        return Result.Failure<T>(AuthErrors.NullSession);
    }
}
