using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs.Auth;

namespace Infomatrix.Core.Application.Features.Auth.RequestResetPassword;

public sealed record RequestResetPasswordCommand(
    string Email)
    : ICommand<EmailDto>;
