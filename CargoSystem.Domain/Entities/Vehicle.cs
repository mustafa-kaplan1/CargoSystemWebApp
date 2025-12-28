using CargoSystem.Domain.Enums;

namespace CargoSystem.Domain.Entities
{
	public class Vehicle
	{
		public int Id { get; set; }
		public VehicleType Type { get; set; }
		private double? _customCapacity;
		public double Capacity
		{
			get => _customCapacity ?? (double)Type;
			set => _customCapacity = value;
		}

		public double CurrentLoad { get; private set; }

		// YENİ EKLENEN: Kalan kapasiteyi kolayca hesaplamak için
		public double RemainingCapacity => Capacity - CurrentLoad;

		public double FuelCostPerKm { get; set; } = 1;
		public double RentalCost { get; set; }

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
