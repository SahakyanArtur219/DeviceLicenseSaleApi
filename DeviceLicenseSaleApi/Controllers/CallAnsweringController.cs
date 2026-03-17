using Microsoft.AspNetCore.Mvc;
using DeviceLicenseSaleApi.Services;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CallAnsweringController : ControllerBase
    {
        private readonly CallAnsweringService _service;

        public CallAnsweringController(CallAnsweringService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpPost]
        public IActionResult Create([FromBody] CallAnswering callAnswering)
        {
            _service.Add(callAnswering);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CallAnswering callAnswering)
        {
            callAnswering.Id = id;
            _service.Update(callAnswering);
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