using CargoSystem.Domain.Entities;

namespace CargoSystem.Application.Services
{
	public class DijkstraPathFinder
	{
		public List<RoadNode> FindShortestPath(
			RoadNode start,
			RoadNode end,
			List<RoadNode> nodes,
			List<RoadEdge> edges)
		{
			var distances = nodes.ToDictionary(n => n.Id, _ => double.MaxValue);
			var previous = new Dictionary<int, int?>();
			var unvisited = new HashSet<int>(nodes.Select(n => n.Id));

			distances[start.Id] = 0;

			while (unvisited.Any())
			{
				var current = unvisited.OrderBy(n => distances[n]).First();
				unvisited.Remove(current);

				if (current == end.Id)
					break;

				var neighbors = edges.Where(e => e.FromNodeId == current);
				foreach (var edge in neighbors)
				{
					var alt = distances[current] + edge.DistanceKm;
					if (alt < distances[edge.ToNodeId])
					{
						distances[edge.ToNodeId] = alt;
						previous[edge.ToNodeId] = current;
					}
				}
			}

			// Path reconstruction
			var path = new List<RoadNode>();
			int? step = end.Id;
			while (step != null)
			{
				path.Insert(0, nodes.First(n => n.Id == step));
				previous.TryGetValue(step.Value, out step);
			}

			return path;
		}
	}
}
