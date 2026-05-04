using SmartWater.Domain.Entities;

namespace SmartWater.Application.Interfaces;

public interface IHydrantService
{
    Task<Hydrant> CreateAsync(string code, string location);
    Task<IReadOnlyList<Hydrant>> GetAllAsync();
    Task<Hydrant?> GetByIdAsync(int id);
    Task<HydrantInspection> AddInspectionAsync(int hydrantId, decimal pressure, decimal flowRate, string inspectedBy, string? notes);
    Task<IReadOnlyList<HydrantInspection>> GetInspectionsByHydrantIdAsync(int hydrantId);
}
