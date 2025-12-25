using CargoSystem.Domain.Entities;

namespace CargoSystem.Domain.Services
{
	public interface IRouteCostCalculator
	{
		void CalculateRouteCost(
			Route route,
			Vehicle vehicle,
			int depotStationId
		);
	}
}
