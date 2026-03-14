using Infomatrix.Core.Domain.Common;
using System.Linq.Expressions;

namespace Infomatrix.Core.Application.Abstractions.Repositories;

public interface IRepository<T>
    where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        T entity,
        CancellationToken cancellationToken = default);

    void Update(T entity);

    void Delete(T entity);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);
}
