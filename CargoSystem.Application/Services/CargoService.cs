using CargoSystem.Application.Services;
using CargoSystem.Domain.Entities;
using CargoSystem.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace CargoSystem.Application.Services
{
	public class CargoService : ICargoService
	{
		private readonly CargoSystemDbContext _context;

		public CargoService(CargoSystemDbContext context)
		{
			_context = context;
		}

		public void CreateCargo(Cargo cargo)
		{
			// Yeni kargo varsayılan olarak "Pending" (Beklemede) statüsünde eklenir.
			cargo.Status = CargoStatus.Pending;
			_context.Cargos.Add(cargo);
			_context.SaveChanges();
		}

		public List<Station> GetStations()
		{
			// Dropdown listesi için istasyonları isme göre sıralı getir
			return _context.Stations.OrderBy(s => s.Name).ToList();
		}
	}
}