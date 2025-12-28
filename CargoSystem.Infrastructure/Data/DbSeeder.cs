using System.Linq;
using CargoSystem.Domain.Entities;
using CargoSystem.Domain.ValueObjects;
using CargoSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore; // Identity Insert için gerekebilir ama şimdilik sırayla ekleyelim

namespace CargoSystem.Infrastructure.Data
{
	public static class DbSeeder
	{
		public static void Seed(CargoSystemDbContext context)
		{
			// Eğer veritabanı boşsa doldur
			if (!context.Stations.Any())
			{
				// ÖNEMLİ: KocaeliRoadGraph ve KocaeliStationsData'daki ID sırasına göre ekliyoruz.
				// Böylece İzmit ID=1, Gebze ID=2 olacak.
				var stations = new Station[]
				{
					new Station { Name = "İzmit",       Location = new Location(40.7650, 29.9400) }, // ID 1 Olmalı
					new Station { Name = "Gebze",       Location = new Location(40.8028, 29.4307) }, // ID 2
					new Station { Name = "Darıca",      Location = new Location(40.7611, 29.3846) }, // ID 3
					new Station { Name = "Çayırova",    Location = new Location(40.8242, 29.3722) }, // ID 4
					new Station { Name = "Gölcük",      Location = new Location(40.7127, 29.8194) }, // ID 5 (Dikkat: Graph dosyanızda Gölcük 5 ise buraya aldım)
					new Station { Name = "Körfez",      Location = new Location(40.7767, 29.7372) }, // ID 6
					new Station { Name = "Derince",     Location = new Location(40.7550, 29.8317) }, // ID 7
					new Station { Name = "Kartepe",     Location = new Location(40.7478, 30.0233) }, // ID 8
					new Station { Name = "Başiskele",   Location = new Location(40.7144, 29.9403) }, // ID 9
                    // Eğer 5 numara Dilovası ise sıralamayı ona göre düzeltin, graph dosyanıza bakın!
                    // Aşağıdakiler kalanlar:
					new Station { Name = "Dilovası",    Location = new Location(40.7856, 29.5444) },
					new Station { Name = "Karamürsel",  Location = new Location(40.6900, 29.6167) },
					new Station { Name = "Kandıra",     Location = new Location(41.0700, 30.1500) }
				};

				// NOT: Graph dosyanızdaki ID'ler ile buradaki ekleme sırası BİREBİR AYNI olmalıdır.
				// Eğer KocaeliRoadGraph.cs dosyanızda ID 5 = Dilovası ise, yukarıdaki listede 5. sıraya Dilovası'nı koyun.

				context.Stations.AddRange(stations);
				context.SaveChanges();
			}

			// Araçları ekle
			if (!context.Vehicles.Any())
			{
				var vehicles = new Vehicle[]
				{
					new Vehicle { Type = (VehicleType)500, IsRented = false, RentalCost = 0 },
					new Vehicle { Type = (VehicleType)750, IsRented = false, RentalCost = 0 },
					new Vehicle { Type = (VehicleType)1000, IsRented = false, RentalCost = 0 }
				};

				context.Vehicles.AddRange(vehicles);
				context.SaveChanges();
			}
		}
	}
}