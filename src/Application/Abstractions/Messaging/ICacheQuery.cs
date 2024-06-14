namespace Application.Abstractions.Messaging;

public interface ICacheQuery<out TResponse> : IQuery<TResponse>, ICacheQuery;

public interface ICacheQuery
{
	public string CacheKey { get; }
	public TimeSpan? Expiration { get; }
}