using FluentValidation;
using Infomatrix.Core.Application.Extensions;

namespace Infomatrix.Core.Application.Features.Auth.Register;

public sealed class RegisterCommandValidator
    : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .ApplyEmailRules();

        RuleFor(x => x.Password)
            .ApplyPasswordRules();
    }
}
