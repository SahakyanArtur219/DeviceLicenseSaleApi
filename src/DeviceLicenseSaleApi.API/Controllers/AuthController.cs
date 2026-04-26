using DeviceLicenseSaleApi.DTOs.Auth;
using DeviceLicenseSaleApi.Logging;
using DeviceLicenseSaleApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeviceLicenseSaleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IActivityLogger _activityLogger;

        public AuthController(IAuthService service, IActivityLogger activityLogger)
        {
            _service = service;
            _activityLogger = activityLogger;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            var currentUserId = GetCurrentUserId();
            var currentUsername = User.Identity?.Name;

            try
            {
                var response = _service.Register(dto);

                await _activityLogger.LogActivityAsync(new ActivityLogEntry
                {
                    Category = "Security",
                    Action = "RegisterUser",
                    Outcome = "Succeeded",
                    UserId = currentUserId,
                    Username = currentUsername,
                    EntityName = "User",
                    EntityId = response.User.Id.ToString(),
                    Description = "Administrator registered a new user.",
                    TraceId = HttpContext.TraceIdentifier,
                    Details = new Dictionary<string, object?>
                    {
                        ["registeredUsername"] = response.User.Username,
                        ["registeredEmail"] = response.User.Email,
                        ["companyId"] = response.User.CompanyId,
                        ["buildingId"] = response.User.BuildingId
                    }
                });

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                await LogRegistrationFailureAsync(currentUserId, currentUsername, dto, ex.Message);
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                await LogRegistrationFailureAsync(currentUserId, currentUsername, dto, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<AuthResponseDto> Login([FromBody] LoginDto dto)
        {
            try
            {
                return Ok(_service.Login(dto));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _activityLogger.LogActivityAsync(new ActivityLogEntry
            {
                Category = "Security",
                Action = "Logout",
                Outcome = "Succeeded",
                UserId = GetCurrentUserId(),
                Username = User.Identity?.Name,
                Description = "User logged out.",
                TraceId = HttpContext.TraceIdentifier
            });

            return Ok(new { message = "Logout recorded." });
        }

        private async Task LogRegistrationFailureAsync(int? currentUserId, string? currentUsername, RegisterDto dto, string reason)
        {
            await _activityLogger.LogActivityAsync(new ActivityLogEntry
            {
                Category = "Security",
                Action = "RegisterUser",
                Outcome = "Failed",
                UserId = currentUserId,
                Username = currentUsername,
                EntityName = "User",
                Description = "Administrator failed to register a new user.",
                TraceId = HttpContext.TraceIdentifier,
                Details = new Dictionary<string, object?>
                {
                    ["reason"] = reason,
                    ["attemptedUsername"] = dto.Username,
                    ["attemptedEmail"] = dto.Email,
                    ["companyId"] = dto.CompanyId,
                    ["buildingId"] = dto.BuildingId
                }
            });
        }

        private int? GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var userId) ? userId : null;
        }
    }
}
