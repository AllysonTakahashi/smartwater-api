namespace SmartWater.API.DTOs.Pressure;

public record CreatePressureReadingRequest(string Sector, decimal Value);
