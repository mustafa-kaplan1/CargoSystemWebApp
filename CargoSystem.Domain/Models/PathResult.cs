namespace CargoSystem.Domain.Models
{
	public class PathResult
	{
		public List<int> StationPath { get; set; } = new();
		public double TotalDistance { get; set; }
	}
}
