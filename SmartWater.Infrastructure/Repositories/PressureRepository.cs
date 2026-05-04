using Microsoft.EntityFrameworkCore;
using SmartWater.Application.Interfaces;
using SmartWater.Domain.Entities;
using SmartWater.Infrastructure.Data;

namespace SmartWater.Infrastructure.Repositories;

public class PressureRepository : IPressureRepository
{
    private readonly AppDbContext _context;

    public PressureRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PressureReading> AddAsync(PressureReading reading)
    {
        await _context.PressureReadings.AddAsync(reading);
        await _context.SaveChangesAsync();
        return reading;
    }

    public Task<PressureReading?> GetLastBySectorAsync(string sector)
        => _context.PressureReadings
            .AsNoTracking()
            .Where(r => r.Sector == sector)
            .OrderByDescending(r => r.Id)
            .FirstOrDefaultAsync();

    public async Task<IReadOnlyList<PressureReading>> GetBySectorAsync(string sector)
        => await _context.PressureReadings
            .AsNoTracking()
            .Where(r => r.Sector == sector)
            .OrderByDescending(r => r.Id)
            .ToListAsync();

    public async Task<IReadOnlyList<PressureReading>> GetTodayAsync()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        return await _context.PressureReadings
            .AsNoTracking()
            .Where(r => r.CreatedAt >= today && r.CreatedAt < tomorrow)
            .ToListAsync();
    }
}
