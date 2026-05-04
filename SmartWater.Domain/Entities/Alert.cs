namespace SmartWater.Domain.Entities;

public class Alert
{
    public int Id { get; private set; }
    public string Sector { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Alert() { }

    public Alert(string sector, string message)
    {
        if (string.IsNullOrWhiteSpace(sector))
            throw new ArgumentException("Sector cannot be empty.", nameof(sector));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.", nameof(message));

        Sector = sector;
        Message = message;
        CreatedAt = DateTime.UtcNow;
    }
}
