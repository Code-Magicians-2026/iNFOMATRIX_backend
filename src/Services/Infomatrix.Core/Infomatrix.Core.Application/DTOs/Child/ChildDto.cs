namespace Infomatrix.Core.Application.DTOs.Child;

public sealed record ChildDto(
    Guid Id,
    string FirstName,
    string LastName,
    int Experience);
