using CargoSystem.Application.Services;
using CargoSystem.Domain.Entities;
using CargoSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoSystem.Web.Controllers
{
	public class CargoController : Controller
	{
		private readonly ICargoService _cargoService;

		public CargoController(ICargoService cargoService)
		{
			_cargoService = cargoService;
		}

		// GET: Kargo Ekleme Formu
		public IActionResult Create()
		{
			var model = new CargoCreateViewModel
			{
				Stations = _cargoService.GetStations()
			};
			return View(model);
		}

		// POST: Kargo Ekleme İşlemi
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(CargoCreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				// ViewModel'den Entity'e dönüştürme
				var cargo = new Cargo
				{
					SenderName = model.SenderName,
					WeightKg = model.WeightKg,
					StationId = model.StationId,
					// Diğer alanlar varsayılan (Status = Pending, vb.)
				};

				_cargoService.CreateCargo(cargo);

				// Başarılı işlem sonrası kullanıcıyı bilgilendir veya yönlendir
				TempData["SuccessMessage"] = "Kargo talebiniz başarıyla alındı.";
				return RedirectToAction("Index", "Home");
			}

			// Hata varsa formu tekrar yükle, istasyon listesini yeniden doldur
			model.Stations = _cargoService.GetStations();
			return View(model);
		}
	}
}