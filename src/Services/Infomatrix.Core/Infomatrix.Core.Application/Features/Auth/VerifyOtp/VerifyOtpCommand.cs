using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs.Auth;

namespace Infomatrix.Core.Application.Features.Auth.VerifyOtp;

public sealed record VerifyOtpCommand(
    string Email,
    string Token)
    : ICommand<EmailDto>;
