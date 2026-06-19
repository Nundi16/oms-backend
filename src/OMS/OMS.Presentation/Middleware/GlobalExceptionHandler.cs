using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OMS.Presentation.Middleware
{
    internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unhandled exception occurred.");
            
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Server Error",
                Detail = "An unexpected error occurred.",
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = MediaTypeNames.Application.ProblemJson;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
