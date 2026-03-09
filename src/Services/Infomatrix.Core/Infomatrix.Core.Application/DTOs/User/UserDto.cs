namespace Infomatrix.Core.Application.DTOs.User;

public sealed record UserDto(
    Guid Id,
    string FullName,
    string Email);
