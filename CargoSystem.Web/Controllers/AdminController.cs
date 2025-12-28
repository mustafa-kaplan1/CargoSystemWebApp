using CargoSystem.Application.DTOs;
using CargoSystem.Application.UseCases;
using CargoSystem.Infrastructure.Data;
using CargoSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CargoSystem.Web.Controllers
{
	public class AdminController : Controller
	{
		private readonly RunScenarioUseCase _useCase;

		public AdminController(RunScenarioUseCase useCase)
		{
			_useCase = useCase;
		}

		public IActionResult Index()
		{
			var stations = KocaeliStationsData.GetStations();

			var vm = new ScenarioViewModel
			{
				Stations = stations.Select(s => new StationScenarioInput
				{
					StationId = s.Id,
					StationName = s.Name
				}).ToList()
			};

			return View(vm);
		}

		[HttpPost]
		public IActionResult RunScenario(ScenarioViewModel vm)
		{
			var request = new ScenarioRequestDto
			{
				AllowRentalVehicles = vm.AllowRentalVehicles,
				DepotStationId = vm.DepotStationId, // View'dan geliyorsa
				Stations = vm.Stations.Select(s => new StationInputDto
				{
					StationId = s.StationId,
					CargoCount = s.CargoCount,
					TotalWeight = s.TotalWeight
				}).ToList()
			};

			var stations = KocaeliStationsData.GetStations(); // İstasyon verilerini al
			var vehicles = DefaultVehicles();

			// UseCase'i çalıştır
			var result = _useCase.Execute(request, stations, vehicles);

			var resultVm = new ScenarioResultViewModel
			{
				TotalCost = result.TotalSystemCost,
				Vehicles = result.Vehicles.Select(v => new VehicleResultVm
				{
					VehicleId = v.VehicleId,
					IsRented = v.IsRented,
					TotalCost = v.TotalCost,
					TotalDistance = v.TotalDistance,
					StationRoute = v.StationRoute,

					// KOORDİNAT EŞLEŞTİRME (YENİ)
					RouteCoordinates = v.StationRoute.Select(routeStationId =>
					{
						var st = stations.FirstOrDefault(s => s.Id == routeStationId);
						return new CoordinateDto
						{
							Lat = st.Location.Latitude,
							Lng = st.Location.Longitude,
							Name = st.Name
						};
					}).ToList()

				}).ToList()
			};

			return View("Result", resultVm);
		}

		private List<Domain.Entities.Vehicle> DefaultVehicles()
		{
			return new()
			{
				new() { Id = 1, Type = Domain.Enums.VehicleType.Small, FuelCostPerKm = 1, RentalCost = 0 },
				new() { Id = 2, Type = Domain.Enums.VehicleType.Medium, FuelCostPerKm = 1, RentalCost = 0 },
				new() { Id = 3, Type = Domain.Enums.VehicleType.Large, FuelCostPerKm = 1, RentalCost = 0 }
			};
		}
	}
}
