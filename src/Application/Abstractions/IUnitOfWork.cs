using System.Data;

namespace Application.Abstractions;

public interface IUnitOfWork
{
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
	IDbTransaction BeginTransaction();
}