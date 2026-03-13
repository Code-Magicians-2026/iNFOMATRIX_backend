namespace Infomatrix.Core.Application.DTOs.Auth;

public sealed record SignUpCacheDto(
    string FirstName,
    string LastName,
    string Password);
