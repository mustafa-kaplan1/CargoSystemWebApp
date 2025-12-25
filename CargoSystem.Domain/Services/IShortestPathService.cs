namespace CargoSystem.Domain.Services
{
	public interface IShortestPathService
	{
		PathResult FindShortestPath(int startStationId, int endStationId);
	}
}
