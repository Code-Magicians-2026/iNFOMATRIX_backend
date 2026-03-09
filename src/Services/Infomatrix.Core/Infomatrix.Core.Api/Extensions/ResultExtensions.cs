using Infomatrix.Core.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Infomatrix.Core.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : result.ToProblemDetails();
    }

    public static IActionResult ToActionResult(this Result result)
    {
        return result.IsSuccess
            ? new NoContentResult()
            : result.ToProblemDetails();
    }

    public static IActionResult ToProblemDetails(this Result result)
    {
        var httpErrorInfo = GetHttpErrorInfo(result.Error.Type);

        var problemDetails = new ProblemDetails
        {
            Type = $"https://httpstatuses.io/{httpErrorInfo.Code}",
            Title = result.Error.Code,
            Detail = result.Error.Message,
            Status = httpErrorInfo.Code,
        };

        if (result is IValidationResult validationResult)
        {
            problemDetails.Extensions = new Dictionary<string, object?>
            {
                {
                    "validationErrors",
                    validationResult.Errors.Select(e => new 
                    { 
                        e.Code,
                        e.Message
                    })
                }
            };
        }

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    private static HttpErrorInfo GetHttpErrorInfo(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.BadRequest => new HttpErrorInfo(400, "Bad Request"),
            ErrorType.Validation => new HttpErrorInfo(400, "Bad Request"),
            ErrorType.NotFound => new HttpErrorInfo(404, "Not found"),
            ErrorType.Conflict => new HttpErrorInfo(409, "Conflict"),
            ErrorType.Unauthorized => new HttpErrorInfo(401, "Unauthorized"),
            ErrorType.Forbidden => new HttpErrorInfo(403, "Forbidden"),
            ErrorType.TooManyRequests => new HttpErrorInfo(429, "Too Many Requests"),
            ErrorType.Unavailable => new HttpErrorInfo(503, "Service Unavailable"),
            _ => new HttpErrorInfo(500, "Server Failure")
        };

    private record struct HttpErrorInfo(int Code, string Title);
}