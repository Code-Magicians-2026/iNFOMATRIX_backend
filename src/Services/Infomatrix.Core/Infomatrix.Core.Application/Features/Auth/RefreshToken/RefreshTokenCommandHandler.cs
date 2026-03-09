using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.Auth.RefreshToken;

internal sealed class RefreshTokenCommandHandler
    : ICommandHandler<RefreshTokenCommand, TokenDto>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<TokenDto>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        return await _authService
            .RefreshTokenAsync(request.AccessToken, request.RefreshToken);
    }
}
