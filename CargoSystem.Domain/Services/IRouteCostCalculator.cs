using CargoSystem.Domain.Entities;

namespace CargoSystem.Domain.Services
{
	public interface IRouteCostCalculator
	{
		void CalculateRouteCost(
			ShippingRoute route,
			Vehicle vehicle,
			int depotStationId
		);
	}
}
