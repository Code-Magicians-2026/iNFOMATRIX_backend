using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.Constants;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Domain.Features.Auth;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.Auth.ResetPassword;

internal sealed class ResetPasswordCommandHandler
    : ICommandHandler<ResetPasswordCommand, TokenDto>
{
    private readonly IAuthService _authService;
    private readonly ICacheService _cacheService;

    public ResetPasswordCommandHandler(
        IAuthService authService,
        ICacheService cacheService)
    {
        _authService = authService;
        _cacheService = cacheService;
    }

    public async Task<Result<TokenDto>> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var token = await _cacheService.GetAsync<TokenDto>(
            key: CacheKeys.ResetPasswordToken(request.Email),
            cancellationToken: cancellationToken);

        if (token is null)
        {
            return Result.Failure<TokenDto>(
                AuthErrors.OtpExpired);
        }

        return await _authService
            .ResetPasswordAsync(request.Email, request.NewPassword, token);
    }
}
