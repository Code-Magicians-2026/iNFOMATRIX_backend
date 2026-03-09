using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs.User;

namespace Infomatrix.Core.Application.Features.User.GetById;

public sealed record GetUserByIdQuery(
    Guid Id)
    : IQuery<UserDto>;
