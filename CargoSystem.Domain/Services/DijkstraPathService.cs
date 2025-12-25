using CargoSystem.Domain.Graph;
using CargoSystem.Domain.Models;

namespace CargoSystem.Domain.Services
{
	public class DijkstraPathService : IShortestPathService
	{
		private readonly Graph.Graph _graph;

		public DijkstraPathService(Graph.Graph graph)
		{
			_graph = graph;
		}

		public PathResult FindShortestPath(int startStationId, int endStationId)
		{
			var distances = new Dictionary<int, double>();
			var previous = new Dictionary<int, int?>();
			var unvisited = new HashSet<int>();

			foreach (var station in _graph.GetAllStations())
			{
				distances[station] = double.MaxValue;
				previous[station] = null;
				unvisited.Add(station);
			}

			distances[startStationId] = 0;

			while (unvisited.Count > 0)
			{
				int current = unvisited
					.OrderBy(s => distances[s])
					.First();

				if (current == endStationId)
					break;

				unvisited.Remove(current);

				foreach (var edge in _graph.GetEdges(current))
				{
					if (!unvisited.Contains(edge.ToStationId))
						continue;

					double alt = distances[current] + edge.DistanceKm;

					if (alt < distances[edge.ToStationId])
					{
						distances[edge.ToStationId] = alt;
						previous[edge.ToStationId] = current;
					}
				}
			}

			return BuildPath(previous, distances, startStationId, endStationId);
		}

		private PathResult BuildPath(
			Dictionary<int, int?> previous,
			Dictionary<int, double> distances,
			int start,
			int end)
		{
			var path = new List<int>();
			int? current = end;

			while (current != null)
			{
				path.Insert(0, current.Value);
				current = previous[current.Value];
			}

			return new PathResult
			{
				StationPath = path,
				TotalDistance = distances[end]
			};
		}
	}
}
