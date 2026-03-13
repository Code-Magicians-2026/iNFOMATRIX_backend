using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Repositories;
using Infomatrix.Core.Application.DTOs.User;
using Infomatrix.Core.Domain.Features.User;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.User.GetById;

internal sealed class GetByIdQueryHandler
    : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;
    
    public GetByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //var user = await _userRepository
        //    .GetByIdAsync(request.Id, cancellationToken);

        //if (user is null)
        //{
        //    return Result.Failure<UserDto>(UserErrors.NotFound(request.Id));
        //}

        //var userDto = new UserDto(
        //    user.Id,
        //    user.FullName,
        //    user.Email);

        //return Result.Success(userDto);
    }
}
