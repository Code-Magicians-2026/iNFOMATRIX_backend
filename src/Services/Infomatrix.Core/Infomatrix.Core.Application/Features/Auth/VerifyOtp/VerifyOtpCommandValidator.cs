using FluentValidation;
using Infomatrix.Core.Application.Extensions;

namespace Infomatrix.Core.Application.Features.Auth.VerifyOtp;

public sealed class VerifyOtpCommandValidator
    : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .ApplyEmailRules();

        RuleFor(x => x.Token)
            .ApplyOtpRules();
    }
}
