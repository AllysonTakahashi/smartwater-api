namespace SmartWater.API.DTOs.Hydrants;

public record HydrantResponse(int Id, string Code, string Location, string Status, DateTime CreatedAt);
