using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs.Auth;

namespace Infomatrix.Core.Application.Features.Auth.ConfirmEmail;

public sealed record ConfirmEmailCommand(
    string Email,
    string Token)
    : ICommand<TokenDto>;
