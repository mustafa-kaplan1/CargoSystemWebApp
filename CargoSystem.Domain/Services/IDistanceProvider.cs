using CargoSystem.Domain.Graph;

namespace CargoSystem.Domain.Services
{
	public interface IDistanceProvider
	{
		Graph GetGraph();
	}
}
