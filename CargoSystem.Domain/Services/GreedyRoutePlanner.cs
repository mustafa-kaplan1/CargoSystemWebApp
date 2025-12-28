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
			var routes = vehicles.Select(v => new ShippingRoute { VehicleId = v.Id }).ToList();

			var targetStations = stations
				.Where(s => s.TotalCargoWeight > 0)
				.Select(s => new
				{
					Station = s,
					Score = CalculateScore(depotStationId, s)
				})
				.OrderBy(x => x.Score)
				.ToList();

			foreach (var item in targetStations)
			{
				bool assigned = false;

				foreach (var vehicle in vehicles)
				{
					if (vehicle.CanLoad(item.Station.TotalCargoWeight))
					{
						vehicle.Load(item.Station.TotalCargoWeight);

						var route = routes.First(r => r.VehicleId == vehicle.Id);
						route.Stations.Add(item.Station);

						assigned = true;
						break;
					}
				}

				if (!assigned && allowRental)
				{
					var rentedVehicle = CreateRentedVehicle(vehicles.Count + 1);
					rentedVehicle.Load(item.Station.TotalCargoWeight);

					vehicles.Add(rentedVehicle);

					routes.Add(new ShippingRoute
					{
						VehicleId = rentedVehicle.Id,
						Stations = new List<Station> { item.Station }
					});
				}
			}

			return routes;
		}

		private double CalculateScore(int depotId, Station station)
		{
			var path = _pathService.FindShortestPath(depotId, station.Id);
			return path.TotalDistance / station.TotalCargoWeight;
		}

		private Vehicle CreateRentedVehicle(int id)
		{
			return new Vehicle
			{
				Id = id,
				Type = Enums.VehicleType.Small,
				RentalCost = 200,
				IsRented = true,
				FuelCostPerKm = 1
			};
		}
	}
}
