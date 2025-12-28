using Microsoft.AspNetCore.Mvc;
using CargoSystem.Application.Services;
using CargoSystem.Domain.Entities;
using CargoSystem.Web.Models;

namespace CargoSystem.Web.Controllers
{
	public class CargoController : Controller
	{
		private readonly ICargoService _cargoService;

		public CargoController(ICargoService cargoService)
		{
			_cargoService = cargoService;
		}

		// GET: /Cargo/Create
		[HttpGet]
		public IActionResult Create()
		{
			// Dropdown için istasyon listesini view model'e dolduruyoruz
			var model = new CargoCreateViewModel
			{
				Stations = _cargoService.GetStations()
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult Create(CargoCreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				var cargo = new Cargo
				{
					// Cargo.cs'ye SenderName eklediğimiz için artık hata vermez
					SenderName = model.SenderName,

					TargetStationId = model.StationId,

					// DÜZELTME: model.WeightKg -> cargo.Weight eşleşmesi yapıldı
					Weight = model.WeightKg,

					// Varsayılan değerler (Opsiyonel)
					Description = "Web Kullanıcısı",
					Status = Domain.Enums.CargoStatus.Pending
				};

				_cargoService.CreateCargo(cargo);

				return RedirectToAction("Index", "Home");
			}

			model.Stations = _cargoService.GetStations();
			return View(model);
		}
	}
}