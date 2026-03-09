using FluentValidation;
using Infomatrix.Core.Application.Extensions;

namespace Infomatrix.Core.Application.Features.Auth.CheckEmail;

public sealed class CheckEmailCommandValidator
    : AbstractValidator<CheckEmailCommand>
{
    public CheckEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .ApplyEmailRules();
    }
}
