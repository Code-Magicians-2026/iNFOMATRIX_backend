using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs.Auth;

namespace Infomatrix.Core.Application.Features.Auth.ResetPassword;

public sealed record ResetPasswordCommand(
    string Email,
    string NewPassword)
    : ICommand<TokenDto>;
