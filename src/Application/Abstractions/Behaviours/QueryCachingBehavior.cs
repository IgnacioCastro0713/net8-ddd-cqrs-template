﻿using Application.Abstractions.Services.Cache;

namespace Application.Abstractions.Behaviours;

public sealed class QueryCachingBehavior<TRequest, TResponse>(ICacheService cacheService)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : ICacheQuery
{
	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		return await cacheService.GetOrCreateAsync(
			request.CacheKey,
			_ => next(),
			request.Expiration,
			cancellationToken);
	}
}