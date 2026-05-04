using Microsoft.EntityFrameworkCore;
using SmartWater.Domain.Entities;
using SmartWater.Infrastructure.Data.Mappings;

namespace SmartWater.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<PressureReading> PressureReadings => Set<PressureReading>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<Hydrant> Hydrants => Set<Hydrant>();
    public DbSet<HydrantInspection> HydrantInspections => Set<HydrantInspection>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMapping());
        modelBuilder.ApplyConfiguration(new PressureReadingMapping());
        modelBuilder.ApplyConfiguration(new AlertMapping());
        modelBuilder.ApplyConfiguration(new HydrantMapping());
        modelBuilder.ApplyConfiguration(new HydrantInspectionMapping());
        modelBuilder.ApplyConfiguration(new InvoiceMapping());
    }
        
}
