using System.Linq;
using CargoSystem.Domain.Entities;
using CargoSystem.Domain.ValueObjects; // Location için gerekli
using CargoSystem.Domain.Enums;        // VehicleType için gerekli

namespace CargoSystem.Infrastructure.Data
{
	public static class DbSeeder
	{
		public static void Seed(CargoSystemDbContext context)
		{
			// context.Database.EnsureCreated(); 

			// 1. İSTASYONLAR (Konum nesnesi kullanılarak güncellendi)
			if (!context.Stations.Any())
			{
				var stations = new Station[]
				{
					new Station { Name = "Başiskele", Location = new Location(40.715, 29.928) },
					new Station { Name = "Çayırova", Location = new Location(40.816, 29.373) },
					new Station { Name = "Darıca", Location = new Location(40.773, 29.400) },
					new Station { Name = "Derince", Location = new Location(40.755, 29.830) },
					new Station { Name = "Dilovası", Location = new Location(40.787, 29.544) },
					new Station { Name = "Gebze", Location = new Location(40.802, 29.430) },
					new Station { Name = "Gölcük", Location = new Location(40.717, 29.821) },
					new Station { Name = "Kandıra", Location = new Location(41.070, 30.150) },
					new Station { Name = "Karamürsel", Location = new Location(40.692, 29.615) },
					new Station { Name = "Kartepe", Location = new Location(40.753, 30.025) },
					new Station { Name = "Körfez", Location = new Location(40.760, 29.740) },
					new Station { Name = "İzmit", Location = new Location(40.765, 29.940) }
				};

				context.Stations.AddRange(stations);
			}

			// 2. ARAÇLAR (VehicleType kullanılarak güncellendi)
			// Not: Vehicle sınıfında PlateNumber yok, Type var.
			// Capacity özelliği Type'tan geliyor (double cast edilerek).
			if (!context.Vehicles.Any())
			{
				var vehicles = new Vehicle[]
				{
					new Vehicle
					{ 
                        // Enum değerlerinizi kontrol edin. Örn: Small=500, Medium=750 varsayılmıştır.
                        // Eğer Enum'da isimler farklıysa lütfen düzeltin.
                        Type = (VehicleType)500,
						IsRented = false,
						RentalCost = 0
					},
					new Vehicle
					{
						Type = (VehicleType)750,
						IsRented = false,
						RentalCost = 0
					},
					new Vehicle
					{
						Type = (VehicleType)1000,
						IsRented = false,
						RentalCost = 0
					}
				};

				context.Vehicles.AddRange(vehicles);
			}

			context.SaveChanges();
		}
	}
}
