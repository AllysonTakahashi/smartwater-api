using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWater.API.DTOs.Hydrants;
using SmartWater.Application.Interfaces;

namespace SmartWater.API.Controllers;

[ApiController]
[Route("api/hydrants")]
[Authorize]
public class HydrantsController : ControllerBase
{
    private readonly IHydrantService _hydrantService;

    public HydrantsController(IHydrantService hydrantService)
    {
        _hydrantService = hydrantService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(HydrantResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<HydrantResponse>> Create([FromBody] CreateHydrantRequest request)
    {
        var hydrant = await _hydrantService.CreateAsync(request.Code, request.Location);
        var response = new HydrantResponse(hydrant.Id, hydrant.Code, hydrant.Location, hydrant.Status.ToString(), hydrant.CreatedAt);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HydrantResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<HydrantResponse>>> GetAll()
    {
        var hydrants = await _hydrantService.GetAllAsync();
        var response = hydrants.Select(h => new HydrantResponse(h.Id, h.Code, h.Location, h.Status.ToString(), h.CreatedAt));
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(HydrantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<HydrantResponse>> GetById(int id)
    {
        var hydrant = await _hydrantService.GetByIdAsync(id);
        if (hydrant is null) return NotFound();
        return Ok(new HydrantResponse(hydrant.Id, hydrant.Code, hydrant.Location, hydrant.Status.ToString(), hydrant.CreatedAt));
    }

    [HttpPost("{id:int}/inspections")]
    [ProducesResponseType(typeof(HydrantInspectionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<HydrantInspectionResponse>> CreateInspection(
        int id, [FromBody] CreateHydrantInspectionRequest request)
    {
        try
        {
            var inspection = await _hydrantService.AddInspectionAsync(
                id, request.Pressure, request.FlowRate, request.InspectedBy, request.Notes);

            var response = new HydrantInspectionResponse(
                inspection.Id, inspection.HydrantId, inspection.Pressure,
                inspection.FlowRate, inspection.InspectedBy, inspection.Notes, inspection.CreatedAt);

            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{id:int}/inspections")]
    [ProducesResponseType(typeof(IEnumerable<HydrantInspectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<HydrantInspectionResponse>>> GetInspections(int id)
    {
        var inspections = await _hydrantService.GetInspectionsByHydrantIdAsync(id);
        var response = inspections.Select(i => new HydrantInspectionResponse(
            i.Id, i.HydrantId, i.Pressure, i.FlowRate, i.InspectedBy, i.Notes, i.CreatedAt));
        return Ok(response);
    }
}
