namespace CargoSystem.Domain.Graph
{
	public class Edge
	{
		public int FromStationId { get; }
		public int ToStationId { get; }
		public double DistanceKm { get; }

		public Edge(int from, int to, double distanceKm)
		{
			FromStationId = from;
			ToStationId = to;
			DistanceKm = distanceKm;
		}
	}
}
