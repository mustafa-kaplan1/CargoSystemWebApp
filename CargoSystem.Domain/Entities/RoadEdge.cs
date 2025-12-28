namespace CargoSystem.Domain.Entities
{
	public class RoadEdge
	{
		public int FromNodeId { get; set; }
		public int ToNodeId { get; set; }
		public double DistanceKm { get; set; }
	}
}
