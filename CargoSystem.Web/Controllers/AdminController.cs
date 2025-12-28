using CargoSystem.Application.DTOs;
using CargoSystem.Application.UseCases;
using CargoSystem.Infrastructure.Data;
using CargoSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using CargoSystem.Application.Services;
using System.Linq;
using System.Collections.Generic;

namespace CargoSystem.Web.Controllers
{
	public class AdminController : Controller
	{
		private readonly RunScenarioUseCase _useCase;
		private readonly ICargoService _cargoService;

		public AdminController(RunScenarioUseCase useCase, ICargoService cargoService)
		{
			_useCase = useCase;
			_cargoService = cargoService;
		}

		public IActionResult Index()
		{
			var stations = _cargoService.GetStations();
			var savedCargos = _cargoService.GetCargos();

			var vm = new ScenarioViewModel
			{
				Stations = stations.Select(s => new StationScenarioInput
				{
					StationId = s.Id,
					StationName = s.Name,
					CargoCount = savedCargos.Count(c => c.TargetStationId == s.Id),
					TotalWeight = savedCargos.Where(c => c.TargetStationId == s.Id).Sum(c => c.Weight)
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
				DepotStationId = vm.DepotStationId,
				Stations = vm.Stations.Select(s => new StationInputDto
				{
					StationId = s.StationId,
					CargoCount = s.CargoCount,
					TotalWeight = s.TotalWeight
				}).ToList()
			};

			var stations = _cargoService.GetStations();
			var vehicles = DefaultVehicles();

			var result = _useCase.Execute(request, stations, vehicles);

			var resultVm = new ScenarioResultViewModel
			{
				TotalCost = result.TotalSystemCost,
				AllStations = stations, // EKLENEN SATIR: Tüm istasyonları view'a gönderiyoruz
				Vehicles = result.Vehicles.Select(v => new VehicleResultVm
				{
					VehicleId = v.VehicleId,
					IsRented = v.IsRented,
					TotalCost = v.TotalCost,
					TotalDistance = v.TotalDistance,
					StationRoute = v.StationRoute,
					RouteCoordinates = v.StationRoute.Select(routeStationId =>
					{
						var st = stations.FirstOrDefault(s => s.Id == routeStationId);
						if (st == null) return null;

						return new CoordinateDto
						{
							Lat = st.Location.Latitude,
							Lng = st.Location.Longitude,
							Name = st.Name
						};
					}).Where(x => x != null).ToList()
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
