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

// OSRM API'sini kullanarak rotayı çizen fonksiyon
async function drawRouteWithOSRM(vehicle, color) {
	if (!vehicle.RouteCoordinates || vehicle.RouteCoordinates.length < 2) return;

	// 1. Koordinatları OSRM formatına (Lng,Lat) çevir ve noktalı virgül ile birleştir
	// Örnek: 29.9167,40.7667;29.4333,40.8000
	var coordinatesString = vehicle.RouteCoordinates.map(function (c) {
		return c.Lng + "," + c.Lat;
	}).join(";");

	// 2. OSRM Public Demo Sunucusuna istek at
	// "overview=full" -> Tam detaylı yol geometrisini istiyoruz
	// "geometries=geojson" -> Sonucu GeoJSON formatında istiyoruz
	var url = `https://router.project-osrm.org/route/v1/driving/${coordinatesString}?overview=full&geometries=geojson`;

	try {
		const response = await fetch(url);
		const data = await response.json();

		if (data.code === 'Ok' && data.routes && data.routes.length > 0) {
			// 3. Gelen GeoJSON koordinatlarını ([Lng, Lat]), Leaflet formatına ([Lat, Lng]) çevir
			var routeCoords = data.routes[0].geometry.coordinates.map(function (coord) {
				return [coord[1], coord[0]];
			});

			// 4. Gerçek yol geometrisini çiz
			var polyline = L.polyline(routeCoords, { color: color, weight: 5, opacity: 0.7 }).addTo(map);

			// Haritayı bu rotaya odakla (İsteğe bağlı, çok araç varsa kapatılabilir)
			// map.fitBounds(polyline.getBounds());
		} else {
			console.warn("OSRM rota bulamadı, düz çizgi çiziliyor...", vehicle.VehicleId);
			drawStraightLineFallback(vehicle, color);
		}
	} catch (error) {
		console.error("OSRM Hatası:", error);
		drawStraightLineFallback(vehicle, color);
	}
}

// OSRM çalışmazsa veya hata verirse yedek olarak düz çizgi çizen fonksiyon
function drawStraightLineFallback(vehicle, color) {
	var latlngs = vehicle.RouteCoordinates.map(function (c) {
		return [c.Lat, c.Lng];
	});
	L.polyline(latlngs, { color: color, weight: 5, opacity: 0.7, dashArray: '10, 10' }).addTo(map);
}

// --- ANA İŞLEM DÖNGÜSÜ ---

if (vehiclesData && vehiclesData.length > 0) {
	vehiclesData.forEach(function (vehicle) {

		// Rengi belirle
		var color = vehicle.IsRented ? 'red' : 'blue';
		if (!vehicle.IsRented) color = getRandomColor();

		// 1. İstasyonlara Marker Ekle (Noktalar hemen gözüksün)
		vehicle.RouteCoordinates.forEach(function (c, index) {
			L.circleMarker([c.Lat, c.Lng], {
				radius: 6,
				fillColor: "#ff7800", // Nokta içi turuncu
				color: "#000",       // Çerçeve siyah
				weight: 1,
				opacity: 1,
				fillOpacity: 0.8
			}).addTo(map).bindPopup(`<b>${c.Name}</b><br>Araç ID: ${vehicle.VehicleId}<br>Sıra: ${index + 1}`);
		});

		// 2. Yolları OSRM ile çizdir (Asenkron çağrı)
		drawRouteWithOSRM(vehicle, color);
	});
}
