using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWater.Domain.Entities;

namespace SmartWater.Infrastructure.Data.Mappings;

public class AlertMapping : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.ToTable("Alerts");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Sector)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Message)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.CreatedAt)
            .IsRequired();
    }
}
