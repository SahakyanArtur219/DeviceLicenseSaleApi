using Microsoft.AspNetCore.Mvc;
using DeviceLicenseSaleApi.Services;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilityController : ControllerBase
    {
        private readonly UtilityService _service;

        public UtilityController(UtilityService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpPost]
        public IActionResult Create([FromBody] Utility entity)
        {
            _service.Add(entity);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Utility entity)
        {
            entity.Id = id;
            _service.Update(entity);
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