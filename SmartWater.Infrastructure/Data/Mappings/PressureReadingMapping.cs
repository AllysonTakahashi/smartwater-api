using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWater.Domain.Entities;

namespace SmartWater.Infrastructure.Data.Mappings;

public class PressureReadingMapping : IEntityTypeConfiguration<PressureReading>
{
    public void Configure(EntityTypeBuilder<PressureReading> builder)
    {
        builder.ToTable("PressureReadings");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Sector)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Value)
            .IsRequired()
            .HasColumnType("decimal(18,4)");

        builder.Property(e => e.CreatedAt)
            .IsRequired();
    }
}
