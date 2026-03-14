using System.ComponentModel.DataAnnotations;

namespace Infomatrix.Core.Infrastructure.AI.Options;

public sealed class AiServiceOptions
{
    public const string SectionName = "AiService";

    [Required]
    public string BaseAddress { get; init; } = string.Empty;
}
