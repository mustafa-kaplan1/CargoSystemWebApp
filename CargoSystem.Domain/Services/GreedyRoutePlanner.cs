using CargoSystem.Domain.Entities;
using CargoSystem.Domain.Services;

namespace CargoSystem.Domain.Services
{
	public class GreedyRoutePlanner : IRoutePlanner
	{
		private readonly IShortestPathService _pathService;

		public GreedyRoutePlanner(IShortestPathService pathService)
		{
			_pathService = pathService;
		}

		public List<ShippingRoute> PlanRoutes(
			List<Station> stations,
			List<Vehicle> vehicles,
			int depotStationId,
			bool allowRental)
		{
			// 1. Mevcut araçlar için boş rotaları oluştur
			var routes = vehicles.Select(v => new ShippingRoute
			{
				VehicleId = v.Id,
				Stations = new List<Station>()
			}).ToList();

			// 2. Dağıtılacak istasyonları belirle (Sadece yükü olanlar)
			// Strateji: Depoya en uzak olanlardan başlamak genellikle daha iyi 'backbone' oluşturur.
			var targetStations = stations
				.Where(s => s.TotalCargoWeight > 0)
				.OrderByDescending(s => _pathService.FindShortestPath(depotStationId, s.Id).TotalDistance)
				.ToList();

			foreach (var station in targetStations)
			{
				ShippingRoute bestRoute = null;
				double minCostIncrease = double.MaxValue;

				// A) MEVCUT ARAÇLARI KONTROL ET
				// Bu kargoyu mevcut araçlardan birine eklemenin maliyeti nedir?
				foreach (var route in routes)
				{
					var vehicle = vehicles.First(v => v.Id == route.VehicleId);

					if (vehicle.CanLoad(station.TotalCargoWeight))
					{
						// İstasyon eklendiğinde oluşacak tahmini maliyet artışı (Distance artışı)
						// Basit yaklaşım: Mevcut rotanın sonuna ekle.
						// Maliyet Artışı = (SonDurak -> YeniDurak)

						int lastStationId = route.Stations.Any() ? route.Stations.Last().Id : depotStationId;
						double distanceIncrease = _pathService.FindShortestPath(lastStationId, station.Id).TotalDistance;

						// Not: Yakıt maliyeti 1 birim olduğu için Distance = Cost kabul edebiliriz.
						if (distanceIncrease < minCostIncrease)
						{
							minCostIncrease = distanceIncrease;
							bestRoute = route;
						}
					}
				}

				// B) KİRALAMA SEÇENEĞİNİ DEĞERLENDİR (Sınırsız Araç Modu)
				bool rented = false;
				if (allowRental)
				{
					// Yeni araç kiralamanın maliyeti: 
					// Kiralama Ücreti (200) + Depo->İstasyon Yakıtı
					double distanceRun = _pathService.FindShortestPath(depotStationId, station.Id).TotalDistance;
					double rentalTotalCost = 200 + distanceRun; // FuelCostPerKm = 1

					// Eğer yeni araç kiralamak, mevcut bir aracı oraya göndermekten daha ucuzsa (veya hiç yer yoksa)
					if (rentalTotalCost < minCostIncrease)
					{
						// -- KİRALAMA YAP --
						var newVehicle = CreateRentedVehicle(GetNextVehicleId(vehicles));

						// Yükü yükle
						if (newVehicle.CanLoad(station.TotalCargoWeight))
						{
							newVehicle.Load(station.TotalCargoWeight);
							vehicles.Add(newVehicle); // Ana listeye ekle

							var newRoute = new ShippingRoute
							{
								VehicleId = newVehicle.Id,
								Stations = new List<Station> { station }
							};
							routes.Add(newRoute);
							rented = true;
						}
					}
				}

				// C) ATAMA YAP (Eğer kiralama yapılmadıysa)
				if (!rented && bestRoute != null)
				{
					var vehicle = vehicles.First(v => v.Id == bestRoute.VehicleId);
					vehicle.Load(station.TotalCargoWeight);
					bestRoute.Stations.Add(station);
				}
			}

			return routes;
		}

		private Vehicle CreateRentedVehicle(int id)
		{
			return new Vehicle
			{
				Id = id,
				Type = Enums.VehicleType.Small, // Kiralık araçlar genelde 500kg (Small) varsayılır
				RentalCost = 200,
				IsRented = true,
				FuelCostPerKm = 1
			};
		}

		private int GetNextVehicleId(List<Vehicle> vehicles)
		{
			return vehicles.Any() ? vehicles.Max(v => v.Id) + 1 : 1;
		}
	}
}