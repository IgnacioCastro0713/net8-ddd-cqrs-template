using System.Linq.Expressions;
using Application.Abstractions.Repositories;

namespace Infrastructure.Persistence.Repositories;

public abstract class Repository<TEntity>(DbContext context)
    : IRepository<TEntity> where TEntity : class
{
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.Set<TEntity>().AsTracking().ToListAsync(cancellationToken);

    public async Task<TEntity?> FindByIdAsync(int id) => await context.Set<TEntity>().FindAsync(id);

    public async Task<TEntity?> FirstByAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) => await context.Set<TEntity>().AnyAsync(predicate, cancellationToken);

    public void Add(TEntity entity) => context.Set<TEntity>().Add(entity);
    public void AddRange(params TEntity[] entities) => context.Set<TEntity>().AddRange(entities);
    public void Update(TEntity entity) => context.Set<TEntity>().Update(entity);
    public void UpdateRange(params TEntity[] entities) => context.Set<TEntity>().UpdateRange(entities);
    public void Remove(TEntity entity) => context.Set<TEntity>().Remove(entity);
}