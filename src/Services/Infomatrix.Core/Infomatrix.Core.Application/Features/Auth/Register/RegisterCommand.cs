using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs.Auth;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.Auth.Register;

public record RegisterCommand(string Email, string Password)
    : ICommand<EmailDto>;

internal sealed class RegisterCommandHandler
    : ICommandHandler<RegisterCommand, EmailDto>
{

    public Task<Result<EmailDto>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {

    }
}