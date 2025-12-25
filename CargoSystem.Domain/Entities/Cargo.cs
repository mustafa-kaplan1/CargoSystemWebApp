namespace CargoSystem.Domain.Entities
{
	public class Cargo
	{
		public int Id { get; set; }
		public int StationId { get; set; }

		public int Quantity { get; set; }
		public double Weight { get; set; }
	}
}
