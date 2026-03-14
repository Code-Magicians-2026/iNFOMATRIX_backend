namespace Infomatrix.Core.Application.DTOs.Child;

public sealed record ChildDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    int Experience);
