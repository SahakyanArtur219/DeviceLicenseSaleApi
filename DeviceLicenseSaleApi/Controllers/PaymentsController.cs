using System.Security.Claims;
using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Services;
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

        public PaymentsController(IPayPalService payPalService, IDeviceService deviceService, ILicenseService licenseService)
        {
            _payPalService = payPalService;
            _deviceService = deviceService;
            _licenseService = licenseService;
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
            var device = _deviceService.GetByIdForUser(dto.DeviceId, userId);

            if (device == null)
            {
                return NotFound(new { message = "Device not found." });
            }

            if (dto.Amount <= 0)
            {
                return BadRequest(new { message = "Checkout total must be greater than zero." });
            }

            try
            {
                var order = await _payPalService.CreateOrderAsync(dto, cancellationToken);
                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(ex.StatusCode.HasValue ? (int)ex.StatusCode.Value : StatusCodes.Status502BadGateway, new { message = ex.Message });
            }
        }

        [HttpPost("orders/{orderId}/capture")]
        public async Task<IActionResult> CaptureOrder(string orderId, [FromBody] PayPalCaptureOrderRequestDto dto, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var device = _deviceService.GetByIdForUser(dto.DeviceId, userId);

            if (device == null)
            {
                return NotFound(new { message = "Device not found." });
            }

            try
            {
                var capture = await _payPalService.CaptureOrderAsync(orderId, cancellationToken);

                if (!string.Equals(capture.Status, "COMPLETED", StringComparison.OrdinalIgnoreCase))
                {
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
                    return NotFound(new { message = "Device could not be updated after payment." });
                }

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
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(ex.StatusCode.HasValue ? (int)ex.StatusCode.Value : StatusCodes.Status502BadGateway, new { message = ex.Message });
            }
            catch (Exception ex)
            {
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
    }
}
