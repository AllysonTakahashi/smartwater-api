namespace SmartWater.Domain.Entities;

public enum InvoiceStatus { Pending, Processing, Processed, Sent, Error }

public class Invoice
{
    public int Id { get; private set; }
    public string Customer { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public DateTime? ProcessingStartedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public DateTime? SentAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Invoice() { }

    public Invoice(string customer, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(customer))
            throw new ArgumentException("Customer cannot be empty.", nameof(customer));

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        Customer = customer;
        Amount = amount;
        Status = InvoiceStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void StartProcessing()
    {
        if (Status != InvoiceStatus.Pending)
            throw new InvalidOperationException("Only pending invoices can start processing.");

        Status = InvoiceStatus.Processing;
        ProcessingStartedAt = DateTime.UtcNow;
    }

    public void MarkAsProcessed()
    {
        if (Status != InvoiceStatus.Processing)
            throw new InvalidOperationException("Invoice must be in Processing status to be marked as Processed.");

        Status = InvoiceStatus.Processed;
        ProcessedAt = DateTime.UtcNow;
    }

    public void MarkAsSent()
    {
        if (Status != InvoiceStatus.Processed)
            throw new InvalidOperationException("Only processed invoices can be marked as sent.");

        Status = InvoiceStatus.Sent;
        SentAt = DateTime.UtcNow;
    }

    public void MarkAsError()
    {
        if (Status == InvoiceStatus.Sent)
            throw new InvalidOperationException("Sent invoices cannot be marked as error.");

        Status = InvoiceStatus.Error;
    }
}
