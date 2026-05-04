using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWater.API.DTOs.Pressure;
using SmartWater.Application.Interfaces;

namespace SmartWater.API.Controllers;

[ApiController]
[Route("api/pressure")]
[Authorize]
public class PressureController : ControllerBase
{
    private readonly IPressureService _pressureService;

    public PressureController(IPressureService pressureService)
    {
        _pressureService = pressureService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PressureReadingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PressureReadingResponse>> Create([FromBody] CreatePressureReadingRequest request)
    {
        var reading = await _pressureService.AddReadingAsync(request.Sector, request.Value);

        var response = new PressureReadingResponse(
            reading.Id,
            reading.Sector,
            reading.Value,
            reading.CreatedAt);

        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpGet("history/{sector}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<PressureReadingResponse>>> GetHistory(string sector)
    {
        var readings = await _pressureService.GetHistoryAsync(sector);

        var response = readings.Select(r => new PressureReadingResponse(
            r.Id,
            r.Sector,
            r.Value,
            r.CreatedAt));

        return Ok(response);
    }

    [HttpGet("dashboard")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<DashboardResponse>> GetDashboard()
    {
        var summary = await _pressureService.GetDashboardAsync();

        var response = new DashboardResponse(
            summary.TotalReadingsToday,
            summary.TotalAlertsToday,
            summary.LastCriticalSector,
            summary.AveragePressureToday);

        return Ok(response);
    }
}
