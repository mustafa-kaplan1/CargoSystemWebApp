namespace CargoSystem.Domain.Graph
{
	public class Graph
	{
		private readonly Dictionary<int, List<Edge>> _adjacencyList = new();

		public void AddEdge(int from, int to, double distanceKm)
		{
			if (!_adjacencyList.ContainsKey(from))
				_adjacencyList[from] = new List<Edge>();

			if (!_adjacencyList.ContainsKey(to))
				_adjacencyList[to] = new List<Edge>();

			_adjacencyList[from].Add(new Edge(from, to, distanceKm));
			_adjacencyList[to].Add(new Edge(to, from, distanceKm)); // çift yönlü
		}

		public List<Edge> GetEdges(int stationId)
		{
			return _adjacencyList.ContainsKey(stationId)
				? _adjacencyList[stationId]
				: new List<Edge>();
		}

		public IEnumerable<int> GetAllStations()
		{
			return _adjacencyList.Keys;
		}
	}
}
