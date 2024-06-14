﻿namespace Application.Abstractions.Services.Cache;

public interface ICacheService
{
	Task<T> GetOrCreateAsync<T>(
		string key,
		Func<CancellationToken, Task<T>> factory,
		TimeSpan? expiration = null,
		CancellationToken cancellationToken = default);

	Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

	Task SetAsync<T>(
		string key,
		T value,
		TimeSpan? expiration = null,
		CancellationToken cancellationToken = default);

	Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}