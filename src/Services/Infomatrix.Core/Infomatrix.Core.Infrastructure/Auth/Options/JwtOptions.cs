using System.ComponentModel.DataAnnotations;

namespace Infomatrix.Core.Infrastructure.Auth.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required(ErrorMessage = "JWT Issuer is required")]
    [Url(ErrorMessage = "JWT Issuer must be a valid URL")]
    public string Issuer { get; init; } = string.Empty;

    [Required(ErrorMessage = "JWT Audience is required")]
    public string Audience { get; init; } = string.Empty;

    [Required(ErrorMessage = "JWT SecretKey is required")]
    [MinLength(32, ErrorMessage = "JWT SecretKey must be at least 32 characters long")]
    public string SecretKey { get; init; } = string.Empty;
}
