using Infomatrix.Core.Api.Features.Users.Responses;
using Infomatrix.Core.Application.DTOs.User;

namespace Infomatrix.Core.Api.Features.Users.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(
        this UserDto dto)
    {
        return new UserResponse(
            dto.Id,
            dto.FullName,
            dto.Email);
    }
}
