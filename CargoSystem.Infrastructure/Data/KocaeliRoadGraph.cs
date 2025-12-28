using CargoSystem.Domain.Entities;

namespace CargoSystem.Infrastructure.Data
{
	public static class KocaeliRoadGraph
	{
		public static List<RoadNode> Nodes = new()
		{
			new() { Id = 1, Name = "İzmit", Latitude = 40.7667, Longitude = 29.9167 },
			new() { Id = 2, Name = "Gebze", Latitude = 40.8000, Longitude = 29.4333 },
			new() { Id = 3, Name = "Darıca", Latitude = 40.7717, Longitude = 29.3700 },
			new() { Id = 4, Name = "Çayırova", Latitude = 40.8233, Longitude = 29.3722 },
			new() { Id = 5, Name = "Gölcük", Latitude = 40.7128, Longitude = 29.8194 },
            // İhtiyaca göre devam edebilirsin
        };

		public static List<RoadEdge> Edges = new()
		{
			new() { FromNodeId = 1, ToNodeId = 5, DistanceKm = 12 },
			new() { FromNodeId = 1, ToNodeId = 2, DistanceKm = 51 },
			new() { FromNodeId = 2, ToNodeId = 3, DistanceKm = 7 },
			new() { FromNodeId = 3, ToNodeId = 4, DistanceKm = 6 },
			new() { FromNodeId = 2, ToNodeId = 4, DistanceKm = 5 },

            // çift yönlü
            new() { FromNodeId = 5, ToNodeId = 1, DistanceKm = 12 },
			new() { FromNodeId = 2, ToNodeId = 1, DistanceKm = 51 },
			new() { FromNodeId = 3, ToNodeId = 2, DistanceKm = 7 },
			new() { FromNodeId = 4, ToNodeId = 3, DistanceKm = 6 },
			new() { FromNodeId = 4, ToNodeId = 2, DistanceKm = 5 },
		};
	}
}
