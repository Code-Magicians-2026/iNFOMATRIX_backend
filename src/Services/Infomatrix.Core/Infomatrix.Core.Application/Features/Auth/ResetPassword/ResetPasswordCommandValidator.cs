using FluentValidation;
using Infomatrix.Core.Application.Extensions;

namespace Infomatrix.Core.Application.Features.Auth.ResetPassword;

public sealed class ResetPasswordCommandValidator
    : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .ApplyEmailRules();

        RuleFor(x => x.NewPassword)
            .ApplyPasswordRules();
    }
}
