using CargoSystem.Application.Services;
using CargoSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace CargoSystem.Web.Controllers
{
	public class RouteController : Controller
	{
		[HttpGet]
		public IActionResult GetRoute(string from, string to)
		{
			var nodes = KocaeliRoadGraph.Nodes;
			var edges = KocaeliRoadGraph.Edges;

			var start = nodes.First(n => n.Name == from);
			var end = nodes.First(n => n.Name == to);

			var dijkstra = new DijkstraPathFinder();
			var path = dijkstra.FindShortestPath(start, end, nodes, edges);

			return Json(path.Select(p => new { p.Latitude, p.Longitude }));
		}
	}
}
