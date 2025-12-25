using CargoSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoSystem.Infrastructure.Data
{
    public class CargoSystemDbContext : DbContext
    {
        public CargoSystemDbContext(DbContextOptions<CargoSystemDbContext> options) : base(options)
        {
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<ShippingRoute> ShippingRoutes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Araç kapasiteleri ve maliyetleri hassas olduğu için decimal/double ayarları
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.RentalCost)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.CostPerKm)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ShippingRoute>()
                .Property(r => r.TotalCost)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}