using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs.Auth;

namespace Infomatrix.Core.Application.Features.Auth.CheckEmail;

public sealed record CheckEmailCommand(
    string Email)
    : ICommand<EmailDto>;
