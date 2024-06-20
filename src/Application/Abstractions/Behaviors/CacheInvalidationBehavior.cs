using Application.Abstractions.Services.Caching;

namespace Application.Abstractions.Behaviors;

public sealed class CacheInvalidationBehavior<TRequest, TResponse>(ICacheService cacheService)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : ICacheInvalidationCommand
{
	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		var response = await next();

		var tasks = request
			.CacheKeysToInvalidate
			.Select(key => cacheService.RemoveAsync(key, cancellationToken));

		await Task.WhenAll(tasks);

		return response;
	}
}