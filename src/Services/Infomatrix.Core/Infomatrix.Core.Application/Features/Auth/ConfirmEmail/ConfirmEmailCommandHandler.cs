using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Repositories;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.Constants;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Domain.Features.Auth;
using Infomatrix.Core.Domain.Features.Parent;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.Auth.ConfirmEmail;

internal sealed class ConfirmEmailCommandHandler
    : ICommandHandler<ConfirmEmailCommand, TokenDto>
{
    private readonly IAuthService _authService;
    private readonly ICacheService _cacheService;
    private readonly IRepository<ParentEntity> _parentRepository;

    public ConfirmEmailCommandHandler(
        IAuthService authService,
        ICacheService cacheService,
        IRepository<ParentEntity> parentRepository)
    {
        _authService = authService;
        _cacheService = cacheService;
        _parentRepository = parentRepository;
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

        var parent = ParentEntity
            .Create(
                result.Value.UserId,
                signUpDto.FirstName,
                signUpDto.LastName,
                request.Email);

        await _parentRepository
            .AddAsync(parent, cancellationToken);
        
        await _parentRepository
            .SaveChangesAsync(cancellationToken);

        return Result
            .Success(result.Value);
    }
}