using System.Diagnostics;
using System.Security.Claims;
using DeviceLicenseSaleApi.Logging;
using DeviceLicenseSaleApi.Services.Interfaces;

namespace DeviceLicenseSaleApi.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IActivityLogger activityLogger)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            await activityLogger.LogRequestAsync(new RequestLogEntry
            {
                Method = context.Request.Method,
                Path = context.Request.Path.Value ?? string.Empty,
                QueryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : null,
                StatusCode = context.Response.StatusCode,
                DurationMs = stopwatch.ElapsedMilliseconds,
                RemoteIpAddress = context.Connection.RemoteIpAddress?.ToString(),
                UserId = TryGetCurrentUserId(context.User),
                Username = context.User.Identity?.Name,
                TraceId = context.TraceIdentifier
            });
        }

        private static int? TryGetCurrentUserId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var userId) ? userId : null;
        }
    }
}
