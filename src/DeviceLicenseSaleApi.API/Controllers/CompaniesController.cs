using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceLicenseSaleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _service;

        public CompaniesController(ICompanyService service)
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
            var company = _service.GetById(id);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public IActionResult Create([FromBody] CompanyCreateDto dto)
        {
            _service.Create(dto);
            return Ok();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] CompanyUpdateDto dto)
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
