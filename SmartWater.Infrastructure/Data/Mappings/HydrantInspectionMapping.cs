using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWater.Domain.Entities;

namespace SmartWater.Infrastructure.Data.Mappings;

public class HydrantInspectionMapping : IEntityTypeConfiguration<HydrantInspection>
{
    public void Configure(EntityTypeBuilder<HydrantInspection> builder)
    {
        builder.ToTable("HydrantInspections");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.HydrantId).IsRequired();
        builder.Property(e => e.Pressure).IsRequired().HasColumnType("decimal(18,4)");
        builder.Property(e => e.FlowRate).IsRequired().HasColumnType("decimal(18,4)");
        builder.Property(e => e.InspectedBy).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Notes).HasMaxLength(500);
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne<Hydrant>()
            .WithMany()
            .HasForeignKey(e => e.HydrantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
