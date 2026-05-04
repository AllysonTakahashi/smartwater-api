namespace SmartWater.API.DTOs.Pressure;

public record PressureReadingResponse(int Id, string Sector, decimal Value, DateTime CreatedAt);
