using CargoSystem.Domain.Entities;
using System.Collections.Generic;

namespace CargoSystem.Application.Services
{
	public interface ICargoService
	{
		List<Station> GetStations();
		void CreateCargo(Cargo cargo);
		List<Cargo> GetCargos();
	}
}