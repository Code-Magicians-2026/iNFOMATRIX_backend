using Infomatrix.Core.Application.Abstractions.Repositories;
using Infomatrix.Core.Domain.Features.User;
using Microsoft.EntityFrameworkCore;

namespace Infomatrix.Core.Infrastructure.Persistence.Repositories;

public class UserRepository
    : Repository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<UserEntity?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(u => u.Email == email)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
