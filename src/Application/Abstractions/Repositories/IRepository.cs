using System.Linq.Expressions;

namespace Application.Abstractions.Repositories;

public interface IRepository<TEntity>
{
    public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<TEntity?> FindByIdAsync(int id);

    public Task<TEntity?> FirstByAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    public void Add(TEntity entity);
    public void Update(TEntity entity);
    public void Remove(TEntity entity);
}