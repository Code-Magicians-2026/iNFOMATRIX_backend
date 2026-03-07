using System.ComponentModel.DataAnnotations;

namespace Infomatrix.Core.Api.Infrastructure.Database;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    [Required(ErrorMessage = "Database ConnectionString is required")]
    [MinLength(10, ErrorMessage = "Database ConnectionString must be at least 10 characters long")]
    public string ConnectionString { get; init; } = string.Empty;
}
