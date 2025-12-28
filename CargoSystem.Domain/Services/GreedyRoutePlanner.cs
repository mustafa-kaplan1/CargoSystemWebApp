using CargoSystem.Domain.Configuration;
using CargoSystem.Domain.Entities;
using CargoSystem.Domain.Services;
using Microsoft.Extensions.Options;

namespace CargoSystem.Domain.Services
{
	public class GreedyRoutePlanner : IRoutePlanner
	{
		private readonly IShortestPathService _pathService;
		private readonly CargoSettings _settings;

		public GreedyRoutePlanner(
			IShortestPathService pathService,
			IOptions<CargoSettings> settings)
		{
			_pathService = pathService;
			_settings = settings.Value;
		}

		public List<ShippingRoute> PlanRoutes(
			List<Station> stations,
			List<Vehicle> vehicles,
			int depotStationId,
			bool allowRental)
		{
			// 1. Mevcut (sabit) araçlar için rota nesnelerini oluştur
			var routes = vehicles.Select(v => new ShippingRoute
			{
				VehicleId = v.Id,
				Stations = new List<Station>(),
				FullPathNodeIds = new List<int>()
			}).ToList();

			// 2. İstasyonları kalan yüke göre takip etmek için bir sözlük oluştur
			var stationDemands = stations
				.Where(s => s.TotalCargoWeight > 0)
				.ToDictionary(s => s.Id, s => s.TotalCargoWeight);

			// 3. İstasyonları merkeze uzaklığına göre sırala (Uzak olan öncelikli)
			var targetStationIds = stations
				.Where(s => s.TotalCargoWeight > 0)
				.OrderByDescending(s => _pathService.FindShortestPath(depotStationId, s.Id).TotalDistance)
				.Select(s => s.Id)
				.ToList();

			foreach (var stationId in targetStationIds)
			{
				var originalStation = stations.First(s => s.Id == stationId);

				// Bu istasyonun yükü bitene kadar dağıtım yap (Splitting)
				while (stationDemands[stationId] > 0)
				{
					double currentCargo = stationDemands[stationId];
					ShippingRoute bestRoute = null;
					double minMarginalCost = double.MaxValue;

					// A) MEVCUT ROTALAR İÇİN MALİYET HESABI
					foreach (var route in routes)
					{
						var vehicle = vehicles.First(v => v.Id == route.VehicleId);

						// Eğer aracın hiç yeri yoksa pas geç
						if (vehicle.RemainingCapacity <= 0) continue;

						int lastNodeId = route.Stations.Any() ? route.Stations.Last().Id : depotStationId;

						// Maliyet Artışı Hesabı: (Son Durak -> Yeni Durak) + (Yeni Durak -> Depo) - (Son Durak -> Depo)

						double distToNew = _pathService.FindShortestPath(lastNodeId, stationId).TotalDistance;
						double distReturnNew = _pathService.FindShortestPath(stationId, depotStationId).TotalDistance;
						double distReturnOld = _pathService.FindShortestPath(lastNodeId, depotStationId).TotalDistance;

						double distanceIncrease = distToNew + distReturnNew - distReturnOld;
						double costIncrease = distanceIncrease * vehicle.FuelCostPerKm;

						if (costIncrease < minMarginalCost)
						{
							minMarginalCost = costIncrease;
							bestRoute = route;
						}
					}

					// B) KİRALAMA SEÇENEĞİ (Yeni Rota Başlatma)
					bool useRental = false;
					// Kiralık araç maliyeti: Kiralama Ücreti + (Depo -> İstasyon -> Depo) * Yakıt
					double rentalDistance = _pathService.FindShortestPath(depotStationId, stationId).TotalDistance * 2;
					double rentalTotalCost = double.MaxValue;

					if (allowRental)
					{
						rentalTotalCost = _settings.RentalCost + (rentalDistance * _settings.CostPerKm);
					}

					// Karar Anı: Mevcut en iyi rotaya eklemek mi daha ucuz, yoksa yeni kiralamak mı?
					if (allowRental && (bestRoute == null || rentalTotalCost < minMarginalCost))
					{
						useRental = true;
					}

					// C) ATAMA İŞLEMİ
					if (useRental)
					{
						// Yeni araç oluştur
						var newVehicle = CreateRentedVehicle(GetNextVehicleId(vehicles));
						vehicles.Add(newVehicle);

						// Yüklenecek miktar: Kargosunun tamamı VEYA aracın kapasitesi kadar
						double amountToLoad = Math.Min(currentCargo, newVehicle.Capacity);
						newVehicle.Load(amountToLoad);
						stationDemands[stationId] -= amountToLoad;

						// Yeni rota oluştur
						var newRoute = new ShippingRoute
						{
							VehicleId = newVehicle.Id,
							Stations = new List<Station> { CreatePartialStation(originalStation, amountToLoad) },
							FullPathNodeIds = new List<int>()
						};

						// Rotayı çiz: Depo -> İstasyon
						var pathResult = _pathService.FindShortestPath(depotStationId, stationId);
						newRoute.FullPathNodeIds.AddRange(pathResult.StationPath);

						routes.Add(newRoute);
					}
					else if (bestRoute != null)
					{
						var vehicle = vehicles.First(v => v.Id == bestRoute.VehicleId);
						int lastNodeId = bestRoute.Stations.Any() ? bestRoute.Stations.Last().Id : depotStationId;

						// Yüklenecek miktar: Kalan kargo VEYA aracın KALAN kapasitesi
						double amountToLoad = Math.Min(currentCargo, vehicle.RemainingCapacity);

						vehicle.Load(amountToLoad);
						stationDemands[stationId] -= amountToLoad;

						// Rotayı güncelle
						var pathResult = _pathService.FindShortestPath(lastNodeId, stationId);

						bestRoute.Stations.Add(CreatePartialStation(originalStation, amountToLoad));

						if (bestRoute.FullPathNodeIds.Count == 0)
							bestRoute.FullPathNodeIds.AddRange(pathResult.StationPath);
						else
							// İlk node (önceki durak) zaten listede var, tekrar eklememek için Skip(1)
							bestRoute.FullPathNodeIds.AddRange(pathResult.StationPath.Skip(1));
					}
					else
					{
						// Hiçbir araca sığmıyor ve kiralama kapalı. Döngüyü kır.
						break;
					}
				}
			}

			return routes;
		}

		// DÜZELTİLEN KISIM BURASIDIR
		private Station CreatePartialStation(Station original, double weight)
		{
			return new Station
			{
				Id = original.Id,
				Name = original.Name,
				Location = original.Location, // HATA ÇÖZÜMÜ: Latitude/Longitude yerine Location nesnesi atandı
				CargoCount = original.CargoCount, // Adet bölünmediği varsayılıyor veya orantılanabilir
				TotalCargoWeight = weight // Sadece bu seferde taşınan ağırlık
			};
		}

		private Vehicle CreateRentedVehicle(int id)
		{
			return new Vehicle
			{
				Id = id,
				Type = Enums.VehicleType.Small,
				RentalCost = _settings.RentalCost,
				IsRented = true,
				FuelCostPerKm = _settings.CostPerKm,
				Capacity = _settings.RentalVehicleCapacity
			};
		}

		private int GetNextVehicleId(List<Vehicle> vehicles)
		{
			return vehicles.Any() ? vehicles.Max(v => v.Id) + 1 : 1;
		}
	}
}