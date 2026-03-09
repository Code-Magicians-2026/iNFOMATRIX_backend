using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs.Auth;

namespace Infomatrix.Core.Application.Features.Auth.RefreshToken;

public sealed record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken)
    : ICommand<TokenDto>;
