using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWater.API.DTOs.Billing;
using SmartWater.Application.Interfaces;
using SmartWater.Domain.Entities;

namespace SmartWater.API.Controllers;

[ApiController]
[Route("api/billing")]
[Authorize]
public class BillingController : ControllerBase
{
    private readonly IBillingService _billingService;

    public BillingController(IBillingService billingService)
    {
        _billingService = billingService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<InvoiceResponse>> Create([FromBody] CreateInvoiceRequest request)
    {
        var invoice = await _billingService.CreateAsync(request.Customer, request.Amount);
        return StatusCode(StatusCodes.Status201Created, ToResponse(invoice));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InvoiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<InvoiceResponse>>> GetAll()
    {
        var invoices = await _billingService.GetAllAsync();
        return Ok(invoices.Select(ToResponse));
    }

    [HttpGet("pending")]
    [ProducesResponseType(typeof(IEnumerable<InvoiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<InvoiceResponse>>> GetPending()
    {
        var invoices = await _billingService.GetPendingAsync();
        return Ok(invoices.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<InvoiceResponse>> GetById(int id)
    {
        var invoice = await _billingService.GetByIdAsync(id);
        if (invoice is null) return NotFound();
        return Ok(ToResponse(invoice));
    }

    [HttpPost("{id:int}/process")]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<InvoiceResponse>> Process(int id)
    {
        try
        {
            var invoice = await _billingService.ProcessInvoiceAsync(id);
            return Ok(ToResponse(invoice));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("{id:int}/send")]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<InvoiceResponse>> Send(int id)
    {
        try
        {
            var invoice = await _billingService.MarkAsSentAsync(id);
            return Ok(ToResponse(invoice));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private static InvoiceResponse ToResponse(Invoice invoice) => new(
        invoice.Id,
        invoice.Customer,
        invoice.Amount,
        invoice.Status.ToString(),
        invoice.ProcessingStartedAt,
        invoice.ProcessedAt,
        invoice.SentAt,
        invoice.CreatedAt);
}
