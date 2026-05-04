using SmartWater.Domain.Entities;

namespace SmartWater.Application.Interfaces;

public interface IBillingService
{
    Task<Invoice> CreateAsync(string customer, decimal amount);
    Task<IReadOnlyList<Invoice>> GetAllAsync();
    Task<Invoice?> GetByIdAsync(int id);
    Task<Invoice> ProcessInvoiceAsync(int id);
    Task<Invoice> MarkAsSentAsync(int id);
    Task<IReadOnlyList<Invoice>> GetPendingAsync();
}
