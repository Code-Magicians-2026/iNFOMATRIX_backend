using FluentValidation;

namespace Infomatrix.Core.Application.Extensions;

public static class RuleBuilderOptionsExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyEmailRules<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .EmailAddress();
    }

    public static IRuleBuilderOptions<T, string> ApplyPasswordRules<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .MinimumLength(8);
    }

    public static IRuleBuilderOptions<T, string> ApplyOtpRules<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Length(6)
            .Matches("^[0-9]{6}$");
    }

    public static IRuleBuilderOptions<T, Guid> ApplyIdRules<T>(
        this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }

    public static IRuleBuilderOptions<T, Dictionary<string, string>> ApplyDictionaryRules<T>(
        this IRuleBuilder<T, Dictionary<string, string>> ruleBuilder)
    {
        var result = ruleBuilder.NotEmpty();

        ruleBuilder.ForEach(x =>
        {
            x.Must(pair => !string.IsNullOrWhiteSpace(pair.Key))
                .WithMessage("Key cannot be empty");

            x.Must(pair => !string.IsNullOrWhiteSpace(pair.Value))
                .WithMessage("Value cannot be empty");
        });

        return result;
    }
}
