using System.Linq;
using CargoSystem.Domain.Entities;

namespace CargoSystem.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void Seed(CargoSystemDbContext context)
        {
            // Veritabanı yoksa oluştur (Migration kullanmıyorsan EnsureCreated, kullanıyorsan Migrate)
            // context.Database.EnsureCreated(); 

            // 1. İSTASYONLAR (Kocaeli İlçeleri) EKLENMEMİŞSE EKLE
            if (!context.Stations.Any())
            {
                var stations = new Station[]
                {
                    new Station { Name = "Başiskele", Latitude = 40.715, Longitude = 29.928 },
                    new Station { Name = "Çayırova", Latitude = 40.816, Longitude = 29.373 },
                    new Station { Name = "Darıca", Latitude = 40.773, Longitude = 29.400 },
                    new Station { Name = "Derince", Latitude = 40.755, Longitude = 29.830 },
                    new Station { Name = "Dilovası", Latitude = 40.787, Longitude = 29.544 },
                    new Station { Name = "Gebze", Latitude = 40.802, Longitude = 29.430 },
                    new Station { Name = "Gölcük", Latitude = 40.717, Longitude = 29.821 },
                    new Station { Name = "Kandıra", Latitude = 41.070, Longitude = 30.150 },
                    new Station { Name = "Karamürsel", Latitude = 40.692, Longitude = 29.615 },
                    new Station { Name = "Kartepe", Latitude = 40.753, Longitude = 30.025 },
                    new Station { Name = "Körfez", Latitude = 40.760, Longitude = 29.740 },
                    new Station { Name = "İzmit", Latitude = 40.765, Longitude = 29.940 }
                };

                context.Stations.AddRange(stations);
            }

            // 2. VARSAYILAN ARAÇLAR EKLENMEMİŞSE EKLE
            if (!context.Vehicles.Any())
            {
                var vehicles = new Vehicle[]
                {
                    new Vehicle 
                    { 
                        PlateNumber = "ARAÇ-1 (Küçük)", 
                        MaxCapacityKg = 500, 
                        IsRented = false, 
                        RentalCost = 0 
                    },
                    new Vehicle 
                    { 
                        PlateNumber = "ARAÇ-2 (Orta)", 
                        MaxCapacityKg = 750, 
                        IsRented = false, 
                        RentalCost = 0 
                    },
                    new Vehicle 
                    { 
                        PlateNumber = "ARAÇ-3 (Büyük)", 
                        MaxCapacityKg = 1000, 
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