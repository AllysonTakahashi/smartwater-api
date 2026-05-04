namespace SmartWater.Domain.Entities;

public class PressureReading
{
    public int Id { get; private set; }
    public string Sector { get; private set; } = string.Empty;
    public decimal Value { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private PressureReading() { }

    public PressureReading(string sector, decimal value)
    {
        if (string.IsNullOrWhiteSpace(sector))
            throw new ArgumentException("Sector cannot be empty.", nameof(sector));

        Sector = sector;
        Value = value;
        CreatedAt = DateTime.UtcNow;
    }

    private const decimal CriticalVariationThreshold = 5m;
    private const decimal MinOperationalPressure = 15m;
    private const decimal MaxOperationalPressure = 30m;

    public bool IsWithinOperationalRange()
        => Value >= MinOperationalPressure && Value <= MaxOperationalPressure;

    public bool HasCriticalVariation(decimal previous)
        => IsWithinOperationalRange() && Math.Abs(Value - previous) >= CriticalVariationThreshold;
}
