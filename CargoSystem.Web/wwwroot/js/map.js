// Haritayı İzmit merkezli başlat
var map = L.map('map').setView([40.7650, 29.9400], 10);

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
	attribution: '&copy; OpenStreetMap contributors'
}).addTo(map);

// Farklı araçlar için rastgele renk üretme fonksiyonu
function getRandomColor() {
	var letters = '0123456789ABCDEF';
	var color = '#';
	for (var i = 0; i < 6; i++) {
		color += letters[Math.floor(Math.random() * 16)];
	}
	return color;
}

// Araç rotalarını çiz
if (vehiclesData && vehiclesData.length > 0) {

	vehiclesData.forEach(function (vehicle) {
		// Rota koordinatlarını al [[lat, lng], [lat, lng], ...] formatına çevir
		var latlngs = vehicle.RouteCoordinates.map(function (c) {
			return [c.Lat, c.Lng];
		});

		if (latlngs.length > 0) {
			var color = vehicle.IsRented ? 'red' : 'blue'; // Kiralıklar kırmızı, mevcutlar mavi olsun
			if (!vehicle.IsRented) color = getRandomColor(); // Veya her araca farklı renk

			// Rotayı çiz (Polyline)
			var polyline = L.polyline(latlngs, { color: color, weight: 5, opacity: 0.7 }).addTo(map);

			// Başlangıç ve bitişe marker koymayalım, tüm istasyonlara küçük daireler koyalım
			vehicle.RouteCoordinates.forEach(function (c, index) {
				L.circleMarker([c.Lat, c.Lng], {
					radius: 5,
					fillColor: "#ff7800",
					color: "#000",
					weight: 1,
					opacity: 1,
					fillOpacity: 0.8
				}).addTo(map).bindPopup("<b>" + c.Name + "</b><br>Araç: " + vehicle.VehicleId);
			});

			// Haritayı rotalara sığdır
			map.fitBounds(polyline.getBounds());
		}
	});
}
