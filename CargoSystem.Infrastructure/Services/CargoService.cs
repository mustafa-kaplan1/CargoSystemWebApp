using CargoSystem.Application.Services; // ICargoService buradan geliyor
using CargoSystem.Domain.Entities;
using CargoSystem.Domain.Enums;
using CargoSystem.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace CargoSystem.Infrastructure.Services
{
	// Namespace artık Infrastructure oldu
	public class CargoService : ICargoService
	{
		private readonly CargoSystemDbContext _context;

		public CargoService(CargoSystemDbContext context)
		{
			_context = context;
		}

		public void CreateCargo(Cargo cargo)
		{
			cargo.Status = CargoStatus.Pending;
			_context.Cargos.Add(cargo);
			_context.SaveChanges();
		}

		public List<Station> GetStations()
		{
			return _context.Stations.OrderBy(s => s.Name).ToList();
		}
	}
}