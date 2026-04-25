using System.Security.Claims;
using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceLicenseSaleApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _service;

        public DevicesController(IDeviceService service)
        {
            _service = service;
        }

        [HttpGet("my")]
        public IActionResult GetMyDevices()
        {
            var userId = GetCurrentUserId();
            var devices = _service.GetByUserId(userId);
            return Ok(devices);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var userId = GetCurrentUserId();
            var device = _service.GetByIdForUser(id, userId);

            if (device == null)
                return NotFound();

            return Ok(device);
        }

        [HttpPost]
        public IActionResult Create([FromBody] DeviceCreateDto dto)
        {
            var userId = GetCurrentUserId();
            var created = _service.CreateForUser(userId, dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] DeviceUpdateDto dto)
        {
            var userId = GetCurrentUserId();
            var updated = _service.UpdateForUser(id, userId, dto);

            if (!updated)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userId = GetCurrentUserId();
            var deleted = _service.DeleteForUser(id, userId);

            if (!deleted)
                return NotFound();

            return Ok();
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(claim);
        }
    }
}