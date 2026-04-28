using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceLicenseSaleApi.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityLogsController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogsController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<ActionResult<ActivityLogSearchResultDto>> Search([FromQuery] ActivityLogQueryDto query, CancellationToken cancellationToken)
        {
            var result = await _activityLogService.SearchAsync(query, cancellationToken);
            return Ok(result);
        }
    }
}
