namespace CargoSystem.Domain.Entities

{
	public class ShippingRoute
	{
		public int VehicleId { get; set; }

		public List<Station> Stations { get; set; } = new();
		public double TotalDistance { get; set; }
		public double TotalCost { get; set; }

		public void CalculateCost(double fuelCostPerKm)
		{
			TotalCost = TotalDistance * fuelCostPerKm;
		}
	}
}