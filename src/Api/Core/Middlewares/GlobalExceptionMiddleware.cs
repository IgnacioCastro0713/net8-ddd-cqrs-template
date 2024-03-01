using Application.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Api.Core.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next)
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers = new()
    {
        { typeof(ValidationException), HandleValidationException },
        { typeof(KeyNotFoundException), HandleNotFoundException },
        { typeof(FileNotFoundException), HandleNotFoundException },
        { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException }
    };

    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
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

    private static async Task HandleValidationException(HttpContext httpContext, Exception ex)
    {
        var exception = (ValidationException)ex;
        httpContext.Response.ContentType = ContentType;
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var response = JsonConvert.SerializeObject(new
        {
            Type = IetfDocRfc.BadRequest,
            Status = StatusCodes.Status400BadRequest,
            Title = "Bad Request",
            Detail = ex.Message,
            exception.Errors
        }, JsonSerializerSettings);

        await httpContext.Response.WriteAsync(response);
    }

    private static async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.ContentType = ContentType;
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        var response = JsonConvert.SerializeObject(new
        {
            Type = IetfDocRfc.NotFound,
            Status = StatusCodes.Status404NotFound,
            Title = "The specified resource was not found.",
            Detail = ex.Message
        }, JsonSerializerSettings);

        await httpContext.Response.WriteAsync(response);
    }

    private static async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.ContentType = ContentType;
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        var response = JsonConvert.SerializeObject(new
        {
            Type = IetfDocRfc.Unauthorized,
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized"
        }, JsonSerializerSettings);

        await httpContext.Response.WriteAsync(response);
    }

    private static async Task HandleInternalServerException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.ContentType = ContentType;
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = JsonConvert.SerializeObject(new
        {
            Type = IetfDocRfc.InternalServerError,
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server",
            Detail = ex.Message
        }, JsonSerializerSettings);

        await httpContext.Response.WriteAsync(response);
    }
}