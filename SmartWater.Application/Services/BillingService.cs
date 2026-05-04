using Microsoft.Extensions.Logging;
using SmartWater.Application.Interfaces;
using SmartWater.Domain.Entities;

namespace SmartWater.Application.Services;

public class BillingService : IBillingService
{
    private readonly IBillingRepository _billingRepository;
    private readonly ILogger<BillingService> _logger;

    public BillingService(IBillingRepository billingRepository, ILogger<BillingService> logger)
    {
        _billingRepository = billingRepository;
        _logger = logger;
    }

    public Task<Invoice> CreateAsync(string customer, decimal amount)
    {
        var invoice = new Invoice(customer, amount);
        _logger.LogInformation("Invoice created for customer '{Customer}': {Amount:C}.", customer, amount);
        return _billingRepository.AddAsync(invoice);
    }

    public Task<IReadOnlyList<Invoice>> GetAllAsync()
        => _billingRepository.GetAllAsync();

    public Task<Invoice?> GetByIdAsync(int id)
        => _billingRepository.GetByIdAsync(id);

    public async Task<Invoice> ProcessInvoiceAsync(int id)
    {
        var invoice = await _billingRepository.GetByIdAsync(id);

        if (invoice is null)
        {
            _logger.LogWarning("Process failed: invoice {Id} not found.", id);
            throw new KeyNotFoundException($"Invoice {id} not found.");
        }

        invoice.StartProcessing();
        invoice.MarkAsProcessed();
        await _billingRepository.UpdateAsync(invoice);

        _logger.LogInformation("Invoice {Id} processed successfully.", id);
        return invoice;
    }

    public async Task<Invoice> MarkAsSentAsync(int id)
    {
        var invoice = await _billingRepository.GetByIdAsync(id);

        if (invoice is null)
        {
            _logger.LogWarning("Send failed: invoice {Id} not found.", id);
            throw new KeyNotFoundException($"Invoice {id} not found.");
        }

        invoice.MarkAsSent();
        await _billingRepository.UpdateAsync(invoice);

        _logger.LogInformation("Invoice {Id} marked as sent.", id);
        return invoice;
    }

    public Task<IReadOnlyList<Invoice>> GetPendingAsync()
        => _billingRepository.GetPendingAsync();
}
