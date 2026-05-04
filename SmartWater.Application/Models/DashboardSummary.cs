namespace SmartWater.Application.Models;

public record DashboardSummary(
    int TotalReadingsToday,
    int TotalAlertsToday,
    string? LastCriticalSector,
    decimal AveragePressureToday);
