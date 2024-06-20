namespace Application.Abstractions.Messaging;

public interface ICacheInvalidationCommand<out TResponse> : ICommand<TResponse>, ICacheInvalidationCommand;

public interface ICacheInvalidationCommand
{
	string[] CacheKeysToInvalidate { get; }
}