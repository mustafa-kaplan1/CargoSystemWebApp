namespace CargoSystem.Application.DTOs
{
	public class VehicleResultDto
	{
		public int VehicleId { get; set; }
		public bool IsRented { get; set; }

		public double TotalDistance { get; set; }
		public double TotalCost { get; set; }

		public List<int> StationRoute { get; set; } = new();
	}
}
