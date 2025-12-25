using CargoSystem.Domain.Entities;
using CargoSystem.Domain.Services;

namespace CargoSystem.Domain.Services
{
	public class RouteCostCalculator : IRouteCostCalculator
	{
		private readonly IShortestPathService _pathService;

		public RouteCostCalculator(IShortestPathService pathService)
		{
			_pathService = pathService;
		}

		public void CalculateRouteCost(
			ShippingRoute route,
			Vehicle vehicle,
			int depotStationId)
		{
			double totalDistance = 0;
			int currentStationId = depotStationId;

			foreach (var station in route.Stations)
			{
				var path = _pathService.FindShortestPath(currentStationId, station.Id);
				totalDistance += path.TotalDistance;

				currentStationId = station.Id;
			}

			// Dönüş (isteğe bağlı – genelde depot’a dönüş eklenir)
			var returnPath = _pathService.FindShortestPath(currentStationId, depotStationId);
			totalDistance += returnPath.TotalDistance;

			route.TotalDistance = totalDistance;

			route.TotalCost =
				(totalDistance * vehicle.FuelCostPerKm)
				+ vehicle.RentalCost;
		}
	}
}
