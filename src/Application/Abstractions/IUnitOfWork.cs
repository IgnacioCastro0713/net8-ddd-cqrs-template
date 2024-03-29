﻿namespace Application.Abstractions;

public interface IUnitOfWork
{
	public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}