using System.Security.Claims;
using DeviceLicenseSaleApi.Logging;
using DeviceLicenseSaleApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeviceLicenseSaleApi.Filters
{
    public class ActionAuditFilter : IAsyncActionFilter
    {
        private readonly IActivityLogger _activityLogger;

        public ActionAuditFilter(IActivityLogger activityLogger)
        {
            _activityLogger = activityLogger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!ShouldAudit(context.HttpContext.Request.Method))
            {
                await next();
                return;
            }

            var executedContext = await next();
            var request = context.HttpContext.Request;
            var routeValues = context.RouteData.Values;
            var statusCode = executedContext.Exception == null
                ? context.HttpContext.Response.StatusCode
                : StatusCodes.Status500InternalServerError;

            await _activityLogger.LogActivityAsync(new ActivityLogEntry
            {
                Category = "Audit",
                Action = $"{request.Method} {routeValues["controller"]}/{routeValues["action"]}",
                Outcome = statusCode >= 400 || executedContext.Exception != null ? "Failed" : "Succeeded",
                UserId = TryGetCurrentUserId(context.HttpContext.User),
                Username = context.HttpContext.User.Identity?.Name,
                EntityName = routeValues["controller"]?.ToString(),
                Description = $"{request.Method} {request.Path}",
                TraceId = context.HttpContext.TraceIdentifier,
                Details = new Dictionary<string, object?>
                {
                    ["statusCode"] = statusCode,
                    ["queryString"] = request.QueryString.HasValue ? request.QueryString.Value : null
                }
            });
        }

        private static bool ShouldAudit(string method)
        {
            return HttpMethods.IsPost(method)
                || HttpMethods.IsPut(method)
                || HttpMethods.IsPatch(method)
                || HttpMethods.IsDelete(method);
        }

        private static int? TryGetCurrentUserId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var userId) ? userId : null;
        }
    }
}
