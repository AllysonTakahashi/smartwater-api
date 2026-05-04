using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWater.Domain.Entities;

namespace SmartWater.Infrastructure.Data.Mappings;

public class InvoiceMapping : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.Customer).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.Property(e => e.ProcessingStartedAt);
        builder.Property(e => e.ProcessedAt);
        builder.Property(e => e.SentAt);
        builder.Property(e => e.CreatedAt).IsRequired();
    }
}
