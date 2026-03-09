using FluentValidation;
using Infomatrix.Core.Application.Extensions;

namespace Infomatrix.Core.Application.Features.Auth.RequestResetPassword;

public sealed class RequestResetPasswordCommandValidator
    : AbstractValidator<RequestResetPasswordCommand>
{
    public RequestResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .ApplyEmailRules();
    }
}
