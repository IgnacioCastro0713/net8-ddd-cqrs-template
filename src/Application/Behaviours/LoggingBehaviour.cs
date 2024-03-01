using Application.Abstractions.Providers;
using Microsoft.Extensions.Logging;

namespace Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(
	ILogger<TRequest> logger,
	IUserContextProvider userContextProvider)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	where TResponse : IResult
{
	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;
		var ntUser = userContextProvider.NtUser ?? string.Empty;

		logger.LogInformation("Handling: {RequestName} {NtUser} {Request}", requestName, ntUser, request);

		var response = await next();

		if (response.IsFailure)
		{
			logger.LogError("Request failure: {RequestName} {NtUser} {Error}", requestName, ntUser, response.Error);
		}

		logger.LogInformation("Handled: {RequestName} {NtUser} {Request}", requestName, ntUser, request);

		return response;
	}
}