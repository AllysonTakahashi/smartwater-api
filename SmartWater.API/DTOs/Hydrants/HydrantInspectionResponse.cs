namespace SmartWater.API.DTOs.Hydrants;

public record HydrantInspectionResponse(
    int Id,
    int HydrantId,
    decimal Pressure,
    decimal FlowRate,
    string InspectedBy,
    string? Notes,
    DateTime CreatedAt);
