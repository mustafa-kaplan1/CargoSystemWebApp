using CargoSystem.Domain.Entities;

namespace CargoSystem.Domain.Services
{
	public interface IRoutePlanner
	{
		List<ShippingRoute> PlanRoutes(
			List<Station> stations,
			List<Vehicle> vehicles,
			int depotStationId,
			bool allowRental
		);
	}
}
