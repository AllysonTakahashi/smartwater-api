using Microsoft.Extensions.Logging;
using SmartWater.Application.Interfaces;
using SmartWater.Application.Models;
using SmartWater.Domain.Entities;

namespace SmartWater.Application.Services;

public class PressureService : IPressureService
{
    private readonly IPressureRepository _pressureRepository;
    private readonly IAlertRepository _alertRepository;
    private readonly ILogger<PressureService> _logger;

    public PressureService(
        IPressureRepository pressureRepository,
        IAlertRepository alertRepository,
        ILogger<PressureService> logger)
    {
        _pressureRepository = pressureRepository;
        _alertRepository = alertRepository;
        _logger = logger;
    }

    public async Task<PressureReading> AddReadingAsync(string sector, decimal value)
    {
        var previous = await _pressureRepository.GetLastBySectorAsync(sector);

        var reading = new PressureReading(sector, value);
        await _pressureRepository.AddAsync(reading);

        _logger.LogInformation("Pressure reading added for sector '{Sector}': {Value} mca.", sector, value);

        if (!reading.IsWithinOperationalRange())
        {
            _logger.LogWarning(
                "Out-of-range pressure reading in sector '{Sector}': {Value} mca. No alert generated.",
                sector, value);
        }
        else if (previous is not null && reading.HasCriticalVariation(previous.Value))
        {
            var previousValue = previous.Value;
            _logger.LogWarning(
                "Critical pressure variation in sector '{Sector}'. Previous: {Previous}, Current: {Current}.",
                sector, previousValue, value);

            var alert = new Alert(sector, $"Critical pressure variation detected in sector {sector}");
            await _alertRepository.AddAsync(alert);
        }

        return reading;
    }

    public Task<IReadOnlyList<PressureReading>> GetHistoryAsync(string sector)
        => _pressureRepository.GetBySectorAsync(sector);

    public async Task<DashboardSummary> GetDashboardAsync()
    {
        var readingsToday = await _pressureRepository.GetTodayAsync();
        var alertsToday = await _alertRepository.GetTodayAsync();

        var average = readingsToday.Count > 0
            ? Math.Round(readingsToday.Average(r => r.Value), 2)
            : 0m;

        return new DashboardSummary(
            TotalReadingsToday: readingsToday.Count,
            TotalAlertsToday: alertsToday.Count,
            LastCriticalSector: alertsToday
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefault()?.Sector,
            AveragePressureToday: average);
    }
}
