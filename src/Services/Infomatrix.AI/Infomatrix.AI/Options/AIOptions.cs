using System.ComponentModel.DataAnnotations;

namespace Infomatrix.AI.Options;

public sealed class AIOptions
{
    public const string SectionName = "AI";

    [Required]
    public string DeploymentName { get; init; } = string.Empty;

    [Required]
    public string Endpoint { get; init; } = string.Empty;

    [Required]
    public string ApiKey { get; init; } = string.Empty;
}
