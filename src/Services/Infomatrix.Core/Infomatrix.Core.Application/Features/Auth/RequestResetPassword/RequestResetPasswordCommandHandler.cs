using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Repositories;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Domain.Features.User;
using Infomatrix.Core.Shared;
using Infomatrix.Core.Shared.Extensions;

namespace Infomatrix.Core.Application.Features.Auth.RequestResetPassword;

internal sealed class RequestResetPasswordCommandHandler
    : ICommandHandler<RequestResetPasswordCommand, EmailDto>
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public RequestResetPasswordCommandHandler(
        IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    public async Task<Result<EmailDto>> Handle(
        RequestResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure<EmailDto>
                (UserErrors.EmailNotFound(request.Email));
        }

        var result = await _authService
            .RequestResetPasswordAsync(request.Email);

        return result.Map(value => new EmailDto(value));
    }
}