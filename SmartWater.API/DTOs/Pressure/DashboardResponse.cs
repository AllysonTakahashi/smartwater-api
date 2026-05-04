namespace SmartWater.API.DTOs.Pressure;

public record DashboardResponse(
    int TotalReadingsToday,
    int TotalAlertsToday,
    string? LastCriticalSector,
    decimal AveragePressureToday);
