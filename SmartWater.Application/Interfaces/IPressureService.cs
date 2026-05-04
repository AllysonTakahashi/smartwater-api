using SmartWater.Application.Models;
using SmartWater.Domain.Entities;

namespace SmartWater.Application.Interfaces;

public interface IPressureService
{
    Task<PressureReading> AddReadingAsync(string sector, decimal value);
    Task<IReadOnlyList<PressureReading>> GetHistoryAsync(string sector);
    Task<DashboardSummary> GetDashboardAsync();
}
