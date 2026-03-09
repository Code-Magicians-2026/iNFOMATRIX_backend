using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs.Auth;

namespace Infomatrix.Core.Application.Features.Auth.Register;

public sealed record RegisterCommand(
    string FullName,
    string Email,
    string Password)
    : ICommand<EmailDto>;
