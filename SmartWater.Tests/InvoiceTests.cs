using FluentAssertions;
using SmartWater.Domain.Entities;

namespace SmartWater.Tests;

public class InvoiceTests
{
    [Fact]
    public void Should_Process_Invoice()
    {
        var invoice = new Invoice("Customer A", 150.00m);

        invoice.StartProcessing();
        invoice.MarkAsProcessed();

        invoice.Status.Should().Be(InvoiceStatus.Processed);
        invoice.ProcessingStartedAt.Should().NotBeNull();
        invoice.ProcessedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Not_Allow_Sending_Before_Processing()
    {
        var invoice = new Invoice("Customer B", 200.00m);

        var act = invoice.MarkAsSent;

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*processed*");
    }
}
