namespace CargoSystem.Web.Models
{
	public class ScenarioViewModel
	{
		public bool AllowRentalVehicles { get; set; }
		public int DepotStationId { get; set; } = 1; // İzmit

		public List<StationScenarioInput> Stations { get; set; } = new();
	}

	public class StationScenarioInput
	{
		public int StationId { get; set; }
		public string StationName { get; set; }

		public int CargoCount { get; set; }
		public double TotalWeight { get; set; }
	}
}
