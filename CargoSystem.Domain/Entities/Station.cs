using CargoSystem.Domain.ValueObjects;

namespace CargoSystem.Domain.Entities
{
	public class Station
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Location Location { get; set; }

		public int CargoCount { get; set; }
		public double TotalCargoWeight { get; set; }

		public Station(int id, string name, Location location)
		{
			Id = id;
			Name = name;
			Location = location;
		}
	}
}
