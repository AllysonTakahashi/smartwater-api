using SmartWater.Domain.Entities;

namespace SmartWater.Application.Interfaces;

public interface IBillingRepository
{
    Task<Invoice> AddAsync(Invoice invoice);
    Task<IReadOnlyList<Invoice>> GetAllAsync();
    Task<Invoice?> GetByIdAsync(int id);
    Task<IReadOnlyList<Invoice>> GetPendingAsync();
    Task UpdateAsync(Invoice invoice);
}
