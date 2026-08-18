using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Task_managment_system.Exceptions;

namespace Task_managment_system.Middlewares
{
    public class AppExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<AppExceptionHandler> _logger;

        public AppExceptionHandler(ILogger<AppExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is OperationCanceledException)
            {
                _logger.LogDebug("request was canceled by the client");
                return true;
            }

            if (exception is AppException appEx)
            {
                _logger.LogWarning(appEx, "AppException {StatusCode}: {Message}", appEx.StatusCode, appEx.Message);

                if (httpContext.Response.HasStarted) return true;

                httpContext.Response.StatusCode = appEx.StatusCode;

                var problem = new ProblemDetails
                {
                    Status = appEx.StatusCode,
                    Title = "request failed",
                    Detail = appEx.Message
                };
                await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
                return true;
            }

            _logger.LogError(exception, "Unhandled exception for {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);
            return false;

        }

    }
}
