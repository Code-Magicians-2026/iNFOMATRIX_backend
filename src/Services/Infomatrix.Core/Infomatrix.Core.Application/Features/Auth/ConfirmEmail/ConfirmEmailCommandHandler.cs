using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Repositories;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.Constants;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Domain.Features.Auth;
using Infomatrix.Core.Domain.Features.User;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.Auth.ConfirmEmail;

internal sealed class ConfirmEmailCommandHandler
    : ICommandHandler<ConfirmEmailCommand, TokenDto>
{
    private readonly IAuthService _authService;
    private readonly ICacheService _cacheService;
    private readonly IUserRepository _userRepository;

    public ConfirmEmailCommandHandler(
        IAuthService authService,
        ICacheService cacheService,
        IUserRepository userRepository)
    {
        _authService = authService;
        _cacheService = cacheService;
        _userRepository = userRepository;
    }

    public async Task<Result<TokenDto>> Handle(
        ConfirmEmailCommand request,
        CancellationToken cancellationToken)
    {
        var signUpDto = await _cacheService.GetAsync<SignUpCacheDto>(
            CacheKeys.SignUp(request.Email),
            cancellationToken);

        if (signUpDto is null)
        {
            return Result.Failure<TokenDto>(AuthErrors.TokenExpired);
        }

        var result = await _authService
            .ConfirmEmailAsync(request.Email, request.Token, signUpDto.Password);

        if (result.IsFailure)
        {
            return result;
        }

        await _cacheService.RemoveAsync(
            CacheKeys.SignUp(request.Email),
            cancellationToken);

        var user = UserEntity
            .Create(
                result.Value.UserId,
                signUpDto.FullName,
                request.Email);

        await _userRepository
            .AddAsync(user, cancellationToken);
        
        await _userRepository
            .SaveChangesAsync(cancellationToken);

        return Result
            .Success(result.Value);
    }
}