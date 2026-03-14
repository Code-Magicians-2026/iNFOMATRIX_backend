using Infomatrix.Core.Api.Features.Child.Responses;
using Infomatrix.Core.Application.DTOs.Child;

namespace Infomatrix.Core.Api.Features.Child.Mappers;

public static class ChildMapper
{
    public static ChildResponse ToResponse(this ChildDto dto)
    {
        return new ChildResponse(
            dto.Id,
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.Experience);
    }
}
