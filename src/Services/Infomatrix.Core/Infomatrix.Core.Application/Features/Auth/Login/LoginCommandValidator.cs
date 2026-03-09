using FluentValidation;
using Infomatrix.Core.Application.Extensions;

namespace Infomatrix.Core.Application.Features.Auth.Login;

public sealed class LoginCommandValidator
    : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .ApplyEmailRules();
    }
}
