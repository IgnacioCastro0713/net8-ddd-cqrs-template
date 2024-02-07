using Microsoft.Extensions.Logging;

namespace Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logger)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
{
	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;

		logger.LogInformation("Handling: {requestName} {Request}", requestName, request);

		var response = await next();

		logger.LogInformation("Handled: {requestName} {Request}", requestName, request);

		return response;
	}
}