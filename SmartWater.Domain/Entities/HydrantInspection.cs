namespace SmartWater.Domain.Entities;

public class HydrantInspection
{
    public int Id { get; private set; }
    public int HydrantId { get; private set; }
    public decimal Pressure { get; private set; }
    public decimal FlowRate { get; private set; }
    public string? Notes { get; private set; }
    public string InspectedBy { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private HydrantInspection() { }

    public HydrantInspection(int hydrantId, decimal pressure, decimal flowRate, string inspectedBy, string? notes = null)
    {
        if (pressure <= 0)
            throw new ArgumentException("Pressure must be greater than zero.", nameof(pressure));

        if (flowRate < 0)
            throw new ArgumentException("FlowRate cannot be negative.", nameof(flowRate));

        if (string.IsNullOrWhiteSpace(inspectedBy))
            throw new ArgumentException("InspectedBy cannot be empty.", nameof(inspectedBy));

        HydrantId = hydrantId;
        Pressure = pressure;
        FlowRate = flowRate;
        Notes = notes;
        InspectedBy = inspectedBy;
        CreatedAt = DateTime.UtcNow;
    }
}
