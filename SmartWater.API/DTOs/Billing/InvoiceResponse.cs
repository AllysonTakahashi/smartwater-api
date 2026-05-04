namespace SmartWater.API.DTOs.Billing;

public record InvoiceResponse(
    int Id,
    string Customer,
    decimal Amount,
    string Status,
    DateTime? ProcessingStartedAt,
    DateTime? ProcessedAt,
    DateTime? SentAt,
    DateTime CreatedAt);
