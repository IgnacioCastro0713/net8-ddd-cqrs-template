using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
	// Register known exception types and handlers.
	private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers = new()
	{
		{ typeof(ValidationException), HandleValidationException },
		{ typeof(KeyNotFoundException), HandleNotFoundException },
		{ typeof(FileNotFoundException), HandleNotFoundException },
		{ typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException }
	};

	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		var exceptionType = exception.GetType();

		if (_exceptionHandlers.TryGetValue(exceptionType, out var handler))
		{
			await handler.Invoke(httpContext, exception);
			return true;
		}

		await HandleInternalServerException(httpContext, exception);

		return false;
	}

	private static async Task HandleValidationException(HttpContext httpContext, Exception ex)
	{
		var exception = (ValidationException)ex;

		httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

		await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors)
		{
			Status = StatusCodes.Status400BadRequest,
			Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
		});
	}

	private static async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
	{
		httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

		await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
		{
			Status = StatusCodes.Status404NotFound,
			Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
			Title = "The specified resource was not found.",
			Detail = ex.Message
		});
	}

	private static async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
	{
		httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

		await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
		{
			Status = StatusCodes.Status401Unauthorized,
			Title = "Unauthorized",
			Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1"
		});
	}

	private static async Task HandleInternalServerException(HttpContext httpContext, Exception ex)
	{
		httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

		await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
		{
			Status = StatusCodes.Status500InternalServerError,
			Title = "InternalServer",
			Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
			Detail = ex.Message
		});
	}
}