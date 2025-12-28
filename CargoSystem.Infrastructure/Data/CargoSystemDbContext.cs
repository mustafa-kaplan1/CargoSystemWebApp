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
			// Configure Location as an Owned Type (Value Object) of Station
			modelBuilder.Entity<Station>()
				.OwnsOne(s => s.Location);

			modelBuilder.Entity<Vehicle>()
				.Property(v => v.RentalCost)
				.HasColumnType("decimal(18,2)");

			// ... existing code ...
			modelBuilder.Entity<Vehicle>()
				.Property(v => v.FuelCostPerKm)
				.HasColumnType("decimal(18,2)");

			modelBuilder.Entity<ShippingRoute>()
				.Property(r => r.TotalCost)
				.HasColumnType("decimal(18,2)");

			base.OnModelCreating(modelBuilder);
		}
	}
}
