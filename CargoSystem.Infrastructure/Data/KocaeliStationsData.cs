using CargoSystem.Domain.Entities;
using CargoSystem.Domain.ValueObjects;

namespace CargoSystem.Infrastructure.Data
{
	public static class KocaeliStationsData
	{
		public static List<Station> GetStations()
		{
			return new List<Station>
			{
				new Station(1, "İzmit",       new Location(40.7650, 29.9400)),
				new Station(2, "Gebze",       new Location(40.8028, 29.4307)),
				new Station(3, "Darıca",      new Location(40.7611, 29.3846)),
				new Station(4, "Çayırova",    new Location(40.8242, 29.3722)),
				new Station(5, "Dilovası",    new Location(40.7856, 29.5444)),
				new Station(6, "Körfez",      new Location(40.7767, 29.7372)),
				new Station(7, "Derince",     new Location(40.7550, 29.8317)),
				new Station(8, "Kartepe",     new Location(40.7478, 30.0233)),
				new Station(9, "Başiskele",   new Location(40.7144, 29.9403)),
				new Station(10,"Gölcük",      new Location(40.7127, 29.8194)),
				new Station(11,"Karamürsel",  new Location(40.6900, 29.6167)),
				new Station(12,"Kandıra",     new Location(41.0700, 30.1500))
			};
		}
	}
}
