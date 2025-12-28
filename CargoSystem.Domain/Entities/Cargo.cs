using CargoSystem.Domain.Enums;

namespace CargoSystem.Domain.Entities
{
	public class Cargo
	{
		public int Id { get; set; }
		public string SenderName { get; set; }
		public string Description { get; set; }
		public double Weight { get; set; }
		public int TargetStationId { get; set; }
		public CargoStatus Status { get; set; }
	}
}