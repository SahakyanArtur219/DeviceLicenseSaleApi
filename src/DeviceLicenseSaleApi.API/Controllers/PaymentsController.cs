using System.Security.Claims;
using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Logging;
using DeviceLicenseSaleApi.Services;
using DeviceLicenseSaleApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceLicenseSaleApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/payments/paypal")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPayPalService _payPalService;
        private readonly IDeviceService _deviceService;
        private readonly ILicenseService _licenseService;
        private readonly IActivityLogger _activityLogger;

        public PaymentsController(IPayPalService payPalService, IDeviceService deviceService, ILicenseService licenseService, IActivityLogger activityLogger)
        {
            _payPalService = payPalService;
            _deviceService = deviceService;
            _licenseService = licenseService;
            _activityLogger = activityLogger;
        }

        [HttpGet("config")]
        public IActionResult GetConfig()
        {
            try
            {
                return Ok(_payPalService.GetClientConfig());
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = ex.Message });
            }
        }

        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrder([FromBody] PayPalCreateOrderRequestDto dto, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var username = User.Identity?.Name;
            var device = _deviceService.GetByIdForUser(dto.DeviceId, userId);

            if (device == null)
            {
                await LogPaymentActivityAsync("CreatePayPalOrder", "Failed", userId, username, dto.DeviceId.ToString(), "Device not found for order creation.");
                return NotFound(new { message = "Device not found." });
            }

            if (dto.Amount <= 0)
            {
                await LogPaymentActivityAsync("CreatePayPalOrder", "Failed", userId, username, dto.DeviceId.ToString(), "Order creation rejected because amount was not greater than zero.");
                return BadRequest(new { message = "Checkout total must be greater than zero." });
            }

            try
            {
                var order = await _payPalService.CreateOrderAsync(dto, cancellationToken);
                await LogPaymentActivityAsync(
                    "CreatePayPalOrder",
                    "Succeeded",
                    userId,
                    username,
                    dto.DeviceId.ToString(),
                    "PayPal order created successfully.",
                    new Dictionary<string, object?>
                    {
                        ["orderId"] = order.Id,
                        ["amount"] = dto.Amount,
                        ["currencyCode"] = dto.CurrencyCode
                    });
                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                await LogPaymentActivityAsync("CreatePayPalOrder", "Failed", userId, username, dto.DeviceId.ToString(), ex.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                await LogPaymentActivityAsync("CreatePayPalOrder", "Failed", userId, username, dto.DeviceId.ToString(), ex.Message);
                return StatusCode(ex.StatusCode.HasValue ? (int)ex.StatusCode.Value : StatusCodes.Status502BadGateway, new { message = ex.Message });
            }
        }

        [HttpPost("orders/{orderId}/capture")]
        public async Task<IActionResult> CaptureOrder(string orderId, [FromBody] PayPalCaptureOrderRequestDto dto, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var username = User.Identity?.Name;
            var device = _deviceService.GetByIdForUser(dto.DeviceId, userId);

            if (device == null)
            {
                await LogPaymentActivityAsync("CapturePayPalOrder", "Failed", userId, username, dto.DeviceId.ToString(), "Device not found for capture.");
                return NotFound(new { message = "Device not found." });
            }

            try
            {
                var capture = await _payPalService.CaptureOrderAsync(orderId, cancellationToken);

                if (!string.Equals(capture.Status, "COMPLETED", StringComparison.OrdinalIgnoreCase))
                {
                    await LogPaymentActivityAsync("CapturePayPalOrder", "Failed", userId, username, dto.DeviceId.ToString(), "PayPal payment was not completed.",
                        new Dictionary<string, object?> { ["orderId"] = orderId, ["status"] = capture.Status });
                    return BadRequest(new { message = "PayPal payment was not completed.", status = capture.Status });
                }

                dto.LicensePayload ??= new LicenseCreateDto();
                dto.LicensePayload.LicenseKey = BuildLicenseKey(dto.LicensePayload.LicenseKey, orderId);
                dto.LicensePayload.IsActive ??= true;
                dto.LicensePayload.ExpiresAt ??= DateTime.UtcNow.AddYears(1);

                var createdLicense = _licenseService.Create(dto.LicensePayload);
                var updated = _deviceService.UpdateForUser(dto.DeviceId, userId, new DeviceUpdateDto
                {
                    DeviceTypeId = dto.DeviceTypeId,
                    LicenseId = createdLicense.Id,
                    Name = dto.DeviceName,
                    Location = dto.DeviceLocation
                });

                if (!updated)
                {
                    await LogPaymentActivityAsync("CapturePayPalOrder", "Failed", userId, username, dto.DeviceId.ToString(), "Device update failed after successful payment.",
                        new Dictionary<string, object?> { ["orderId"] = orderId, ["licenseId"] = createdLicense.Id });
                    return NotFound(new { message = "Device could not be updated after payment." });
                }

                await LogPaymentActivityAsync(
                    "CapturePayPalOrder",
                    "Succeeded",
                    userId,
                    username,
                    dto.DeviceId.ToString(),
                    "PayPal payment captured and license assigned successfully.",
                    new Dictionary<string, object?>
                    {
                        ["orderId"] = capture.OrderId,
                        ["captureId"] = capture.CaptureId,
                        ["licenseId"] = createdLicense.Id
                    });

                return Ok(new
                {
                    capture.OrderId,
                    capture.CaptureId,
                    capture.Status,
                    capture.PayerEmail,
                    LicenseId = createdLicense.Id,
                    DeviceId = dto.DeviceId
                });
            }
            catch (InvalidOperationException ex)
            {
                await LogPaymentActivityAsync("CapturePayPalOrder", "Failed", userId, username, dto.DeviceId.ToString(), ex.Message,
                    new Dictionary<string, object?> { ["orderId"] = orderId });
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                await LogPaymentActivityAsync("CapturePayPalOrder", "Failed", userId, username, dto.DeviceId.ToString(), ex.Message,
                    new Dictionary<string, object?> { ["orderId"] = orderId });
                return StatusCode(ex.StatusCode.HasValue ? (int)ex.StatusCode.Value : StatusCodes.Status502BadGateway, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                await LogPaymentActivityAsync("CapturePayPalOrder", "Failed", userId, username, dto.DeviceId.ToString(), ex.Message,
                    new Dictionary<string, object?> { ["orderId"] = orderId });
                return BadRequest(new { message = ex.Message });
            }
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(claim!);
        }

        private static string BuildLicenseKey(string? existingLicenseKey, string orderId)
        {
            var baseKey = string.IsNullOrWhiteSpace(existingLicenseKey)
                ? "PAYPAL-LICENSE"
                : existingLicenseKey.Trim();

            return $"{baseKey}-{orderId}";
        }

        private Task LogPaymentActivityAsync(
            string action,
            string outcome,
            int userId,
            string? username,
            string entityId,
            string description,
            Dictionary<string, object?>? details = null)
        {
            return _activityLogger.LogActivityAsync(new ActivityLogEntry
            {
                Category = "Payment",
                Action = action,
                Outcome = outcome,
                UserId = userId,
                Username = username,
                EntityName = "Device",
                EntityId = entityId,
                Description = description,
                TraceId = HttpContext.TraceIdentifier,
                Details = details
            });
        }
    }
}
