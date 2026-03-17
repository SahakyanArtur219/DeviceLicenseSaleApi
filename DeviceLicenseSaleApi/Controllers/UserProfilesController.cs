using Microsoft.AspNetCore.Mvc;
using DeviceLicenseSaleApi.Services;
using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileService _service;

        public UserProfilesController(IUserProfileService service)
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
            var profile = _service.GetById(id);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpGet("by-user/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            var profile = _service.GetByUserId(userId);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserProfileCreateDto dto)
        {
            _service.Create(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserProfileUpdateDto dto)
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