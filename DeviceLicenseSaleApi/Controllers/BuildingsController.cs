using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var building = _service.GetById(id);

            if (building == null)
            {
                return NotFound();
            }

            return Ok(building);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public IActionResult Create([FromBody] BuildingCreateDto dto)
        {
            _service.Create(dto);
            return Ok();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] BuildingUpdateDto dto)
        {
            _service.Update(id, dto);
            return Ok();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}
