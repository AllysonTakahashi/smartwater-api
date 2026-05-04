namespace SmartWater.API.DTOs.Hydrants;

public record CreateHydrantInspectionRequest(decimal Pressure, decimal FlowRate, string InspectedBy, string? Notes);
