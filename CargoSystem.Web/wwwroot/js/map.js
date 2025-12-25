var map = L.map('map').setView([40.7650, 29.9400], 9);

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
	attribution: '&copy; OpenStreetMap'
}).addTo(map);

// Burada backend’ten gelen rota noktaları çizilir
// Polyline sadece görsel amaçlıdır
