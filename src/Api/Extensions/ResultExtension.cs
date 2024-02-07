using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Api.Extensions;

public static class ResultExtension
{
    public static IActionResult ToOk<TResult>(this Result<TResult> result) => result.Match(Success, Failure);
    public static IActionResult ToOk(this Result result) => result.Match(Success, Failure);
    private static OkObjectResult Success<TResult>(TResult next) => new(next);
    private static OkResult Success() => new();
    private static IActionResult Failure(Error error) => Response(error);

    private static ObjectResult Response(Error error)
    {
        var response = new
        {
            Title = error.StatusCode.ToString(),
            Type = GetType(error.StatusCode),
            Errors = new[]
            {
                new
                {
                    error.Code, error.Description
                }
            }
        };

        return new ObjectResult(response)
        {
            StatusCode = (int)error.StatusCode
        };
    }

	/// <summary>
	/// Register known Error types.
	/// To more details consult: https://datatracker.ietf.org/doc/html/rfc7231
	/// </summary>
	/// <param name="code"></param>
	/// <returns></returns>
	private static string GetType(HttpStatusCode code) => code switch
    {
        HttpStatusCode.BadRequest => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
        HttpStatusCode.NotFound => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
        HttpStatusCode.Conflict => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
        _ => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
    };

    public static IActionResult ToDownload<TFileStream>(
        this Result<TFileStream> result,
        string? fileDownloadName = default
    ) where TFileStream : FileStream => result.Match(fileStream =>
    {
        ArgumentNullException.ThrowIfNull(fileStream);
        FileExtensionContentTypeProvider provider = new();
        var filePath = fileStream.Name;
        var fileName = Path.GetFileName(filePath);

        if (!provider.TryGetContentType(filePath, out var contentType))
            contentType = MediaTypeNames.Application.Octet;

        if (!string.IsNullOrEmpty(fileDownloadName) && !string.IsNullOrWhiteSpace(fileDownloadName))
            fileName = fileDownloadName;

        return new FileStreamResult(fileStream, contentType) { FileDownloadName = fileName };
    }, Failure);
}