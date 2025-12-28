using CargoSystem.Domain.Entities; // Station sınıfı için gerekli

namespace CargoSystem.Web.Models
{
	public class ScenarioResultViewModel
	{
		public double TotalCost { get; set; }
		public List<VehicleResultVm> Vehicles { get; set; } = new();

		// EKLENEN KISIM: Haritada tüm istasyonları göstermek için
		public List<Station> AllStations { get; set; } = new();
	}

	public class VehicleResultVm
	{
		public int VehicleId { get; set; }
		public bool IsRented { get; set; }
		public double TotalCost { get; set; }
		public double TotalDistance { get; set; }
		public List<int> StationRoute { get; set; }

		public List<CoordinateDto> RouteCoordinates { get; set; } = new();
	}

	public class CoordinateDto
	{
		public double Lat { get; set; }
		public double Lng { get; set; }
		public string Name { get; set; }
	}
}
