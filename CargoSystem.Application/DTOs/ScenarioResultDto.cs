namespace CargoSystem.Application.DTOs
{
	public class ScenarioResultDto
	{
		public double TotalSystemCost { get; set; }
		public double TotalDistance { get; set; }

		public List<VehicleResultDto> Vehicles { get; set; } = new();
	}
}
