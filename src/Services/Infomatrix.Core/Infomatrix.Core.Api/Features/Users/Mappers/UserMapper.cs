using Infomatrix.Core.Domain.Entities;
using Infomatrix.Core.Api.Features.Users.Requests;
using Infomatrix.Core.Api.Features.Users.Responses;

namespace Infomatrix.Core.Api.Features.Users.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(this UserEntity entity)
    {
        return new UserResponse(
            entity.Id,
            entity.FullName);
    }

    public static UserEntity ToEntity(this CreateUserRequest request)
    {
        return UserEntity.Create(request.FullName);
    }

    public static IEnumerable<UserResponse> ToResponse(this IEnumerable<UserEntity> entities)
    {
        return entities.Select(e => e.ToResponse());
    }
}
