using Infomatrix.Core.Domain.Features.User;

namespace Infomatrix.Core.Application.Abstractions.Repositories;

public interface IUserRepository
    : IRepository<UserEntity>
{
    Task<UserEntity?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);
}
