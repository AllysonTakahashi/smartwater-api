using Microsoft.EntityFrameworkCore;
using SmartWater.Application.Interfaces;
using SmartWater.Domain.Entities;
using SmartWater.Infrastructure.Data;

namespace SmartWater.Infrastructure.Repositories;

public class HydrantRepository : IHydrantRepository
{
    private readonly AppDbContext _context;

    public HydrantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Hydrant> AddAsync(Hydrant hydrant)
    {
        await _context.Hydrants.AddAsync(hydrant);
        await _context.SaveChangesAsync();
        return hydrant;
    }

    public async Task<IReadOnlyList<Hydrant>> GetAllAsync()
        => await _context.Hydrants.AsNoTracking().OrderBy(h => h.Code).ToListAsync();

    public Task<Hydrant?> GetByIdAsync(int id)
        => _context.Hydrants.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);

    public async Task<HydrantInspection> AddInspectionAsync(HydrantInspection inspection)
    {
        await _context.HydrantInspections.AddAsync(inspection);
        await _context.SaveChangesAsync();
        return inspection;
    }

    public async Task<IReadOnlyList<HydrantInspection>> GetInspectionsByHydrantIdAsync(int hydrantId)
        => await _context.HydrantInspections
            .AsNoTracking()
            .Where(i => i.HydrantId == hydrantId)
            .OrderByDescending(i => i.Id)
            .ToListAsync();
}
