using CargoSystem.Application.DTOs;
using CargoSystem.Domain.Entities;
using CargoSystem.Domain.Services;

namespace CargoSystem.Application.UseCases
{
	public class RunScenarioUseCase
	{
		private readonly IRoutePlanner _routePlanner;
		private readonly IRouteCostCalculator _costCalculator;

		public RunScenarioUseCase(
			IRoutePlanner routePlanner,
			IRouteCostCalculator costCalculator)
		{
			_routePlanner = routePlanner;
			_costCalculator = costCalculator;
		}

		public ScenarioResultDto Execute(
			ScenarioRequestDto request,
			List<Station> stations,
			List<Vehicle> vehicles)
		{
			// Senaryo verilerini stationlara işle
			foreach (var input in request.Stations)
			{
				var station = stations.First(s => s.Id == input.StationId);
				station.CargoCount = input.CargoCount;
				station.TotalCargoWeight = input.TotalWeight;
			}

			var routes = _routePlanner.PlanRoutes(
				stations,
				vehicles,
				request.DepotStationId,
				request.AllowRentalVehicles
			);

			var result = new ScenarioResultDto();

			foreach (var route in routes)
			{
				var vehicle = vehicles.First(v => v.Id == route.VehicleId);

				_costCalculator.CalculateRouteCost(
					route,
					vehicle,
					request.DepotStationId
				);

				result.Vehicles.Add(new VehicleResultDto
				{
					VehicleId = vehicle.Id,
					IsRented = vehicle.IsRented,
					TotalCost = route.TotalCost,
					TotalDistance = route.TotalDistance,
					StationRoute = route.Stations.Select(s => s.Id).ToList()
				});

				result.TotalSystemCost += route.TotalCost;
				result.TotalDistance += route.TotalDistance;
			}

			return result;
		}
	}
}
