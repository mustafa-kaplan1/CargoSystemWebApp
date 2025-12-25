namespace CargoSystem.Application.DTOs
{
	public class ScenarioRequestDto
	{
		public bool AllowRentalVehicles { get; set; }
		public int DepotStationId { get; set; }

		public List<StationInputDto> Stations { get; set; } = new();
	}

	public class StationInputDto
	{
		public int StationId { get; set; }
		public int CargoCount { get; set; }
		public double TotalWeight { get; set; }
	}
}
