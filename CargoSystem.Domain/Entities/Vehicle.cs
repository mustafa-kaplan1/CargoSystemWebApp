using CargoSystem.Domain.Enums;

namespace CargoSystem.Domain.Entities
{
	public class Vehicle
	{
		public int Id { get; set; }
		public VehicleType Type { get; set; }

		public double Capacity => (double)Type;
		public double CurrentLoad { get; private set; }

		public double FuelCostPerKm { get; set; } = 1; // sabit
		public double RentalCost { get; set; }        // 0 veya 200

		public bool IsRented { get; set; }

		public bool CanLoad(double weight)
		{
			return CurrentLoad + weight <= Capacity;
		}

		public void Load(double weight)
		{
			if (!CanLoad(weight))
				throw new InvalidOperationException("Araç kapasitesi aşıldı");

			CurrentLoad += weight;
		}
	}
}
