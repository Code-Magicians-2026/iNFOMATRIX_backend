using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Repositories;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.Constants;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.Auth.Register;

internal sealed class RegisterCommandHandler
    : ICommandHandler<RegisterCommand, EmailDto>
{
    private readonly IAuthService _authService;
    private readonly ICacheService _cacheService;

    public RegisterCommandHandler(
        IAuthService authService,
        ICacheService cacheService)
    {
        _authService = authService;
        _cacheService = cacheService;
    }

    public async Task<Result<EmailDto>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _authService
            .RegisterUserAsync(request.Email, request.Password);

        if (result.IsFailure)
        {
            return Result.Failure<EmailDto>(result.Error);
        }

        var signUpDto = new SignUpCacheDto(
            request.FirstName,
            request.LastName,
            request.Password);

        await _cacheService.SetAsync(
            key: CacheKeys.SignUp(request.Email),
            value: signUpDto,
            expiry: TimeSpan.FromMinutes(10),
            cancellationToken: cancellationToken);

        return Result.Success(new EmailDto(result.Value));
    }
}
