using DeviceLicenseSaleApi.Application.Exceptions;
using DeviceLicenseSaleApi.Logging;
using DeviceLicenseSaleApi.Services.Interfaces;
using System.Net;
using System.Text.Json;

namespace DeviceLicenseSaleApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IActivityLogger activityLogger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception while processing {Method} {Path}. TraceId: {TraceId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.TraceIdentifier);

                await activityLogger.LogActivityAsync(new ActivityLogEntry
                {
                    Category = "Error",
                    Action = "UnhandledException",
                    Outcome = "Failed",
                    UserId = TryGetCurrentUserId(context.User),
                    Username = context.User.Identity?.Name,
                    Description = $"{context.Request.Method} {context.Request.Path}",
                    TraceId = context.TraceIdentifier,
                    Details = new Dictionary<string, object?>
                    {
                        ["exceptionType"] = exception.GetType().FullName,
                        ["message"] = exception.Message
                    }
                });

                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                ValidationException => (HttpStatusCode.BadRequest, exception.Message),
                NotFoundException => (HttpStatusCode.NotFound, exception.Message),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, exception.Message),
                InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var payload = JsonSerializer.Serialize(new
            {
                message
            });

            return context.Response.WriteAsync(payload);
        }

        private static int? TryGetCurrentUserId(System.Security.Claims.ClaimsPrincipal user)
        {
            var claim = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var userId) ? userId : null;
        }
    }
}
