using System;

namespace CargoSystem.Domain.Entities
{
	public class Cargo : BaseEntity
	{
		public int StationId { get; set; } // Hangi ilçeye gidecek
		public Station Station { get; set; } // İlişki

		public string SenderName { get; set; }
		public double WeightKg { get; set; }

		public CargoStatus Status { get; set; } = CargoStatus.Pending;

		public int? ShippingRouteId { get; set; } // Hangi sefer (rota) ile taşındığı
		public ShippingRoute ShippingRoute { get; set; }
	}

	public enum CargoStatus
	{
		Pending,
		Planned,
		Delivered
	}
}