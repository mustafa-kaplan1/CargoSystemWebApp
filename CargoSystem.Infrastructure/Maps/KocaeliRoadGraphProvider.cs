using CargoSystem.Domain.Graph;
using CargoSystem.Domain.Services;

namespace CargoSystem.Infrastructure.Maps
{
	public class KocaeliRoadGraphProvider : IDistanceProvider
	{
		public Graph GetGraph()
		{
			var graph = new Graph();

			// İzmit merkez bağlantıları
			graph.AddEdge(1, 7, 8);   // İzmit - Derince
			graph.AddEdge(1, 6, 15);  // İzmit - Körfez
			graph.AddEdge(1, 8, 12);  // İzmit - Kartepe
			graph.AddEdge(1, 9, 10);  // İzmit - Başiskele
			graph.AddEdge(1, 10, 14); // İzmit - Gölcük

			// Batı hattı
			graph.AddEdge(6, 5, 18);  // Körfez - Dilovası
			graph.AddEdge(5, 2, 12);  // Dilovası - Gebze
			graph.AddEdge(2, 3, 6);   // Gebze - Darıca
			graph.AddEdge(2, 4, 5);   // Gebze - Çayırova

			// Güney hattı
			graph.AddEdge(10, 11, 22); // Gölcük - Karamürsel

			// Kartepe - Kandıra
			graph.AddEdge(8, 12, 30);

			return graph;
		}
	}
}
