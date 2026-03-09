using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.Auth.Login;

internal sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand, TokenDto>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<Result<TokenDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        return _authService
            .LoginAsync(request.Email, request.Password);
    }
}