using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Application.Extensions;

public static class ResultExtension
{
    public static IActionResult ToOk<TResult>(this Result<TResult> result) => result.Match(Success, Failure);
    private static OkObjectResult Success<TResult>(TResult next) => new(next);
    private static IActionResult Failure(Error error) => Response(error);

    private static ObjectResult Response(Error error)
    {
        var response = new
        {
            Type = GetType(error.StatusCode),
            Status = (int)error.StatusCode,
            Title = error.StatusCode.ToString(),
            Error = new { error.Code, error.Description }
        };

        return new ObjectResult(response)
        {
            StatusCode = (int)error.StatusCode
        };
    }

    private static string GetType(HttpStatusCode code) => code switch
    {
        HttpStatusCode.BadRequest => IetfDocRfc.BadRequest,
        HttpStatusCode.NotFound => IetfDocRfc.NotFound,
        HttpStatusCode.Conflict => IetfDocRfc.Conflict,
        _ => IetfDocRfc.InternalServerError
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