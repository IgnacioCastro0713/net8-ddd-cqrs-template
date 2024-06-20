using System.Text.Json;
using Application.Exceptions;

namespace Api.Core.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next)
{
	private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers = new()
	{
		{ typeof(ValidationException), (ctx, ex) => HandleValidationException(ctx, (ValidationException)ex) },
		{ typeof(KeyNotFoundException), HandleNotFoundException },
		{ typeof(FileNotFoundException), HandleNotFoundException },
		{ typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException }
	};

	private static readonly JsonSerializerOptions JsonSerializerSettings = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	private const string ContentType = "application/json";

	public async Task InvokeAsync(HttpContext httpContext)
	{
		try
		{
			await next(httpContext);
		}
		catch (Exception exception)
		{
			var exceptionType = exception.GetType();

			if (_exceptionHandlers.TryGetValue(exceptionType, out var handler))
			{
				await handler.Invoke(httpContext, exception);
				return;
			}

			await HandleInternalServerException(httpContext, exception);
		}
	}

	private static async Task HandleValidationException(HttpContext httpContext, ValidationException ex)
	{
		var response = CreateResponse(
			IetfDocRfc.BadRequest,
			StatusCodes.Status400BadRequest,
			"Bad Request", 
			ex.Message,
			ex.Errors);
		await WriteResponse(httpContext, StatusCodes.Status400BadRequest, response);
	}

	private static async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
	{
		var response = CreateResponse(
			IetfDocRfc.NotFound,
			StatusCodes.Status404NotFound,
			"The specified resource was not found.",
			ex.Message);
		await WriteResponse(httpContext, StatusCodes.Status404NotFound, response);
	}

	private static async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
	{
		var response = CreateResponse(
			IetfDocRfc.Unauthorized,
			StatusCodes.Status401Unauthorized,
			"Unauthorized",
			ex.Message);
		await WriteResponse(httpContext, StatusCodes.Status401Unauthorized, response);
	}

	private static async Task HandleInternalServerException(HttpContext httpContext, Exception ex)
	{
		var response = CreateResponse(
			IetfDocRfc.InternalServerError,
			StatusCodes.Status500InternalServerError,
			"Internal Server",
			ex.Message);
		await WriteResponse(httpContext, StatusCodes.Status500InternalServerError, response);
	}

	private static object CreateResponse(
		string type,
		int status,
		string title,
		string? detail = null,
		Dictionary<string, string[]>? errors = null)
	{
		return new
		{
			Type = type,
			Status = status,
			Title = title,
			Detail = detail,
			Errors = errors
		};
	}

	private static async Task WriteResponse(HttpContext httpContext, int statusCode, object response)
	{
		httpContext.Response.ContentType = ContentType;
		httpContext.Response.StatusCode = statusCode;
		await httpContext.Response.WriteAsJsonAsync(response, JsonSerializerSettings);
	}
}