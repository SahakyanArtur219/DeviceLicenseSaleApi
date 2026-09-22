using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Logging;
using DeviceLicenseSaleApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeviceLicenseSaleApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/device-recommendations")]
    public class DeviceRecommendationController : ControllerBase
    {
        private readonly IDeviceRecommendationService _deviceRecommendationService;
        private readonly IActivityLogger _activityLogger;

        public DeviceRecommendationController(
            IDeviceRecommendationService deviceRecommendationService,
            IActivityLogger activityLogger)
        {
            _deviceRecommendationService = deviceRecommendationService;
            _activityLogger = activityLogger;
        }

        [HttpPost("chat")]
        public async Task<ActionResult<DeviceRecommendationChatResponseDto>> Chat(
            [FromBody] DeviceRecommendationChatRequestDto request,
            CancellationToken cancellationToken)
        {
            var response = _deviceRecommendationService.Recommend(request);

            await _activityLogger.LogActivityAsync(new ActivityLogEntry
            {
                Category = "Recommendation",
                Action = "DeviceTypeAssistant",
                Outcome = "Succeeded",
                UserId = GetCurrentUserId(),
                Username = User.Identity?.Name,
                Description = "User requested a device type recommendation.",
                TraceId = HttpContext.TraceIdentifier,
                Details = new Dictionary<string, object?>
                {
                    ["message"] = request.Message,
                    ["summary"] = response.Summary,
                    ["recommendationCount"] = response.Recommendations.Count
                }
            }, cancellationToken);

            return Ok(response);
        }

        private int? GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var userId) ? userId : null;
        }
    }
}
