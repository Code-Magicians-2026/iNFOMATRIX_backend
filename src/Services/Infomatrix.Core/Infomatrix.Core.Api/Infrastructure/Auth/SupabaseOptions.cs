using System.ComponentModel.DataAnnotations;

namespace Infomatrix.Core.Api.Infrastructure.Auth;

public sealed class SupabaseOptions
{
    public const string SectionName = "Supabase";

    [Required(ErrorMessage = "Supabase URL is required")]
    [Url(ErrorMessage = "Supabase URL must be a valid URL")]
    public string Url { get; init; } = string.Empty;

    [Required(ErrorMessage = "Supabase Key is required")]
    public string Key { get; init; } = string.Empty;

    [Required(ErrorMessage = "Supabase AdminKey is required")]
    public string AdminKey { get; init; } = string.Empty;
}
