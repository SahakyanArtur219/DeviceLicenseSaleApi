using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceLicenseSaleApi.Controllers;

[Authorize]
[ApiController]
[Route("api/weekly-bundles")]
public class WeeklyBundlesController : ControllerBase
{
    private readonly IWeeklyBundleService _weeklyBundleService;

    public WeeklyBundlesController(IWeeklyBundleService weeklyBundleService)
    {
        _weeklyBundleService = weeklyBundleService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_weeklyBundleService.GetAll());
    }

    [HttpGet("active/device-types/{deviceTypeId:int}")]
    public IActionResult GetActiveForDeviceType(int deviceTypeId)
    {
        return Ok(_weeklyBundleService.GetActiveForDeviceType(deviceTypeId));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Create([FromBody] WeeklyBundleCreateDto dto)
    {
        return Ok(_weeklyBundleService.Create(dto));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _weeklyBundleService.Delete(id);
        return NoContent();
    }
}