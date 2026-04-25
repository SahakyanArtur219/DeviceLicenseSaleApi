using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeviceLicenseSaleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceTypesController : ControllerBase
    {
        private readonly DeviceTypesService _service;

        public DeviceTypesController(DeviceTypesService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var deviceType = _service.GetById(id);
            return deviceType == null ? NotFound() : Ok(deviceType);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateDeviceTypesDto dto)
        {
            var created = _service.Add(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] CreateDeviceTypesDto dto)
        {
            var updated = _service.Update(id, dto);
            return updated ? Ok() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}
