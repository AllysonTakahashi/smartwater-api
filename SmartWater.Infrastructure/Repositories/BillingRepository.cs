using Microsoft.EntityFrameworkCore;
using SmartWater.Application.Interfaces;
using SmartWater.Domain.Entities;
using SmartWater.Infrastructure.Data;

namespace SmartWater.Infrastructure.Repositories;

public class BillingRepository : IBillingRepository
{
    private readonly AppDbContext _context;

    public BillingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice> AddAsync(Invoice invoice)
    {
        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();
        return invoice;
    }

    public async Task<IReadOnlyList<Invoice>> GetAllAsync()
        => await _context.Invoices.AsNoTracking().OrderByDescending(i => i.Id).ToListAsync();

    public Task<Invoice?> GetByIdAsync(int id)
        => _context.Invoices.FirstOrDefaultAsync(i => i.Id == id);

    public async Task<IReadOnlyList<Invoice>> GetPendingAsync()
        => await _context.Invoices
            .AsNoTracking()
            .Where(i => i.Status == InvoiceStatus.Pending)
            .OrderByDescending(i => i.Id)
            .ToListAsync();

    public async Task UpdateAsync(Invoice invoice)
    {
        await _context.SaveChangesAsync();
    }
}
