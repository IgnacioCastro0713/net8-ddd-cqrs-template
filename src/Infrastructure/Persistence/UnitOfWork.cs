using System.Data;
using Application.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence;

public sealed class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
	public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		await context.SaveChangesAsync(cancellationToken);
	}

	public IDbTransaction BeginTransaction()
	{
		var transaction = context.Database.BeginTransaction();

		return transaction.GetDbTransaction();
	}
}