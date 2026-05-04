using SmartWater.Domain.Entities;

namespace SmartWater.Application.Interfaces;

public interface IPressureRepository
{
    Task<PressureReading> AddAsync(PressureReading reading);
    Task<PressureReading?> GetLastBySectorAsync(string sector);
    Task<IReadOnlyList<PressureReading>> GetBySectorAsync(string sector);
    Task<IReadOnlyList<PressureReading>> GetTodayAsync();
}
