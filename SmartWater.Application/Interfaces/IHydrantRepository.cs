using SmartWater.Domain.Entities;

namespace SmartWater.Application.Interfaces;

public interface IHydrantRepository
{
    Task<Hydrant> AddAsync(Hydrant hydrant);
    Task<IReadOnlyList<Hydrant>> GetAllAsync();
    Task<Hydrant?> GetByIdAsync(int id);
    Task<HydrantInspection> AddInspectionAsync(HydrantInspection inspection);
    Task<IReadOnlyList<HydrantInspection>> GetInspectionsByHydrantIdAsync(int hydrantId);
}
