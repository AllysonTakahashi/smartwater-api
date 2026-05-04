using SmartWater.Domain.Entities;

namespace SmartWater.Application.Interfaces;

public interface IAlertRepository
{
    Task AddAsync(Alert alert);
    Task<IReadOnlyList<Alert>> GetTodayAsync();
}
