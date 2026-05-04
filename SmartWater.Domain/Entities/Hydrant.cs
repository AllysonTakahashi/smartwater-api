namespace SmartWater.Domain.Entities;

public enum HydrantStatus { Active, Inactive, UnderMaintenance }

public class Hydrant
{
    public int Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public HydrantStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Hydrant() { }

    public Hydrant(string code, string location)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty.", nameof(code));

        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location cannot be empty.", nameof(location));

        Code = code;
        Location = location;
        Status = HydrantStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(HydrantStatus status) => Status = status;
}
