using Microsoft.EntityFrameworkCore;
using SmartWater.Application.Interfaces;
using SmartWater.Domain.Entities;
using SmartWater.Infrastructure.Data;

namespace SmartWater.Infrastructure.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly AppDbContext _context;

    public AlertRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Alert alert)
    {
        await _context.Alerts.AddAsync(alert);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Alert>> GetTodayAsync()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        return await _context.Alerts
            .AsNoTracking()
            .Where(a => a.CreatedAt >= today && a.CreatedAt < tomorrow)
            .ToListAsync();
    }
}
