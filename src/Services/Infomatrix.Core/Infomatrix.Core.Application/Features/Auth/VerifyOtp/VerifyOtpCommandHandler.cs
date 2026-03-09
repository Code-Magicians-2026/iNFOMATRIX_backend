using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.Constants;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Shared;
using Infomatrix.Core.Shared.Extensions;

namespace Infomatrix.Core.Application.Features.Auth.VerifyOtp;

internal sealed class VerifyOtpCommandHandler
    : ICommandHandler<VerifyOtpCommand, EmailDto>
{
    private readonly IAuthService _authService;
    private readonly ICacheService _cacheService;

    public VerifyOtpCommandHandler(
        IAuthService authService,
        ICacheService cacheService)
    {
        _authService = authService;
        _cacheService = cacheService;
    }

    public async Task<Result<EmailDto>> Handle(
        VerifyOtpCommand request,
        CancellationToken cancellationToken)
    {
        Result<TokenDto> result = await _authService
            .VerifyOtpAsync(request.Email, request.Token);

        if (result.IsFailure)
        {
            return Result.Failure<EmailDto>(result.Error);
        }

        await _cacheService.SetAsync(
            key: CacheKeys.ResetPasswordToken(request.Email),
            value: result.Value,
            expiry: TimeSpan.FromMinutes(5),
            cancellationToken: cancellationToken);

        return result.Map(r => new EmailDto(request.Email));
    }

    Task<Result<EmailDto>> ICommandHandler<VerifyOtpCommand, EmailDto>.Handle(VerifyOtpCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
