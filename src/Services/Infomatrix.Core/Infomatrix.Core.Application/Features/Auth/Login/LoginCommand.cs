using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs.Auth;

namespace Infomatrix.Core.Application.Features.Auth.Login;

public sealed record LoginCommand(
    string Email,
    string Password)
    : ICommand<TokenDto>;
