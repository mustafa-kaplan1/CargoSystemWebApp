using System;
using System.Collections.Generic;

namespace CargoSystem.Domain.Entities
{
	public class ShippingRoute : BaseEntity
	{
		public int VehicleId { get; set; }
		public Vehicle Vehicle { get; set; }

		public double TotalDistanceKm { get; set; }
		public decimal TotalCost { get; set; }

		// İzlenen rota sırası (Örn: "Uni -> Izmit -> Gebze -> Uni" şeklinde string veya JSON tutulabilir)
		public string RoutePathJson { get; set; }

		// Bu seferde taşınan kargolar
		public ICollection<Cargo> Cargos { get; set; }
	}
}