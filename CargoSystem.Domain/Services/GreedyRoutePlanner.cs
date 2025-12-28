using CargoSystem.Domain.Configuration; // YENİ EKLENEN (Settings yerine Configuration)
using CargoSystem.Domain.Entities;
using CargoSystem.Domain.Services;
using Microsoft.Extensions.Options; // Options pattern için gerekli

namespace CargoSystem.Domain.Services
{
	public class GreedyRoutePlanner : IRoutePlanner
	{
		private readonly IShortestPathService _pathService;
		private readonly CargoSettings _settings; // Ayarları tutacak değişken

		// Constructor'a IOptions<CargoSettings> ekliyoruz
		public GreedyRoutePlanner(
			IShortestPathService pathService,
			IOptions<CargoSettings> settings)
		{
			_pathService = pathService;
			_settings = settings.Value; // Ayarları içeri alıyoruz
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
				Stations = new List<Station>(),
				FullPathNodeIds = new List<int>()
			}).ToList();

			// 2. Dağıtılacak istasyonları belirle
			var targetStations = stations
				.Where(s => s.TotalCargoWeight > 0)
				.OrderByDescending(s => _pathService.FindShortestPath(depotStationId, s.Id).TotalDistance)
				.ToList();

			foreach (var station in targetStations)
			{
				ShippingRoute bestRoute = null;
				double minCostIncrease = double.MaxValue;

				// A) MEVCUT ARAÇLARI KONTROL ET
				foreach (var route in routes)
				{
					var vehicle = vehicles.First(v => v.Id == route.VehicleId);

					if (vehicle.CanLoad(station.TotalCargoWeight))
					{
						int lastStationId = route.Stations.Any() ? route.Stations.Last().Id : depotStationId;
						double distanceIncrease = _pathService.FindShortestPath(lastStationId, station.Id).TotalDistance;

						// Mesafe * KmBaşıMaliyet (Config'den geliyor)
						double costIncrease = distanceIncrease * _settings.CostPerKm;

						if (costIncrease < minCostIncrease)
						{
							minCostIncrease = costIncrease;
							bestRoute = route;
						}
					}
				}

				// B) KİRALAMA SEÇENEĞİNİ DEĞERLENDİR
				bool rented = false;
				if (allowRental)
				{
					double distanceRun = _pathService.FindShortestPath(depotStationId, station.Id).TotalDistance;

					// Config'den okunan değerler: (Kiralama Ücreti) + (Mesafe * KmMaliyeti)
					double rentalTotalCost = _settings.RentalCost + (distanceRun * _settings.CostPerKm);

					if (rentalTotalCost < minCostIncrease)
					{
						// -- KİRALAMA YAP --
						var newVehicle = CreateRentedVehicle(GetNextVehicleId(vehicles));

						if (newVehicle.CanLoad(station.TotalCargoWeight))
						{
							newVehicle.Load(station.TotalCargoWeight);
							vehicles.Add(newVehicle);

							var newRoute = new ShippingRoute
							{
								VehicleId = newVehicle.Id,
								Stations = new List<Station> { station },
								FullPathNodeIds = new List<int>()
							};

							var pathResult = _pathService.FindShortestPath(depotStationId, station.Id);
							newRoute.FullPathNodeIds.AddRange(pathResult.StationPath);

							routes.Add(newRoute);
							rented = true;
						}
					}
				}

				// C) ATAMA YAP
				if (!rented && bestRoute != null)
				{
					var vehicle = vehicles.First(v => v.Id == bestRoute.VehicleId);
					int lastStationId = bestRoute.Stations.Any() ? bestRoute.Stations.Last().Id : depotStationId;

					var pathResult = _pathService.FindShortestPath(lastStationId, station.Id);

					vehicle.Load(station.TotalCargoWeight);
					bestRoute.Stations.Add(station);

					if (bestRoute.FullPathNodeIds.Count == 0)
						bestRoute.FullPathNodeIds.AddRange(pathResult.StationPath);
					else
						bestRoute.FullPathNodeIds.AddRange(pathResult.StationPath.Skip(1));
				}
			}

			return routes;
		}

		private Vehicle CreateRentedVehicle(int id)
		{
			// Değerler artık Config'den geliyor
			return new Vehicle
			{
				Id = id,
				Type = Enums.VehicleType.Small,
				RentalCost = _settings.RentalCost,
				IsRented = true,
				FuelCostPerKm = _settings.CostPerKm,
				Capacity = _settings.RentalVehicleCapacity // Kapasiteyi de set ediyoruz
			};
		}

		private int GetNextVehicleId(List<Vehicle> vehicles)
		{
			return vehicles.Any() ? vehicles.Max(v => v.Id) + 1 : 1;
		}
	}
}