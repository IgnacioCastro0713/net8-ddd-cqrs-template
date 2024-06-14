using System.Linq.Expressions;

namespace Application.Abstractions.Repositories;

public interface IRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TEntity?> FindByIdAsync(int id);

    Task<TEntity?> FirstByAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    void Add(TEntity entity);
    void AddRange(params TEntity[] entity);

    void Update(TEntity entity);
    void UpdateRange(params TEntity[] entity);
    void Remove(TEntity entity);
}