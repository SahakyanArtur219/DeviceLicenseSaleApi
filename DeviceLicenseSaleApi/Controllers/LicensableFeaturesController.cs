using Microsoft.AspNetCore.Mvc;
using DeviceLicenseSaleApi.Services;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicensableFeaturesController : ControllerBase
    {
        private readonly LicensableFeaturesService _service;

        public LicensableFeaturesController(LicensableFeaturesService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpPost]
        public IActionResult Create([FromBody] LicensableFeatures entity)
        {
            _service.Add(entity);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] LicensableFeatures entity)
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