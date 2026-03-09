using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Repositories;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Domain.Features.User;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.Auth.CheckEmail;

internal sealed class CheckEmailCommandHandler
    : ICommandHandler<CheckEmailCommand, EmailDto>
{
    private readonly IUserRepository _userRepository;

    public CheckEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<EmailDto>> Handle(
        CheckEmailCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByEmailAsync(request.Email, cancellationToken);

        return user is not null
            ? Result.Failure<EmailDto>(UserErrors.AlreadyExists(request.Email))
            : Result.Success(new EmailDto(request.Email));
    }
}
