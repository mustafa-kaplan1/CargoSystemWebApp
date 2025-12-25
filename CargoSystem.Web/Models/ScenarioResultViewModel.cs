namespace CargoSystem.Web.Models
{
	public class ScenarioResultViewModel
	{
		public double TotalCost { get; set; }

		public List<VehicleResultVm> Vehicles { get; set; } = new();
	}

	public class VehicleResultVm
	{
		public int VehicleId { get; set; }
		public bool IsRented { get; set; }

		public double TotalDistance { get; set; }
		public double TotalCost { get; set; }

		public List<int> StationRoute { get; set; } = new();
	}
}
