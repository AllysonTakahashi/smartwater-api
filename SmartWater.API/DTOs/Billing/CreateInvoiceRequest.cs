namespace SmartWater.API.DTOs.Billing;

public record CreateInvoiceRequest(string Customer, decimal Amount);
