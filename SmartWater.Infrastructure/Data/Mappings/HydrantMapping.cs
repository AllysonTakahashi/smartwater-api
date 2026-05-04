using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWater.Domain.Entities;

namespace SmartWater.Infrastructure.Data.Mappings;

public class HydrantMapping : IEntityTypeConfiguration<Hydrant>
{
    public void Configure(EntityTypeBuilder<Hydrant> builder)
    {
        builder.ToTable("Hydrants");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Location)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasIndex(e => e.Code)
            .IsUnique();
    }
}
