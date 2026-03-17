using Microsoft.AspNetCore.Mvc;
using DeviceLicenseSaleApi.Services;
using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingService _service;

        public BuildingsController(IBuildingService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var building = _service.GetById(id);

            if (building == null)
                return NotFound();

            return Ok(building);
        }

        [HttpPost]
        public IActionResult Create([FromBody] BuildingCreateDto dto)
        {
            _service.Create(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] BuildingUpdateDto dto)
        {
            _service.Update(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}