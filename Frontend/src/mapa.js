document.addEventListener("DOMContentLoaded", () => {
  const mapElement = document.getElementById("map");
  if (!mapElement) return;

  // 1. Inicializar el mapa centrado en Morón (-34.6534, -58.6198) con zoom 13
  const map = L.map("map").setView([-34.6534, -58.6198], 13);

  // 2. Cargar las capas de mapa gratuitas de OpenStreetMap
  L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
    maxZoom: 19,
    attribution: '© <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
  }).addTo(map);

  // 3. Datos de los reportes (en producción esto se consulta desde tu backend)
  const reportes = [
    {
      titulo: "Bache peligroso",
      categoria: "Bache o calle deteriorada",
      lat: -34.6534,
      lng: -58.6198,
      estado: "En proceso"
    },
    {
      titulo: "Falta de alumbrado",
      categoria: "Alumbrado público",
      lat: -34.6580,
      lng: -58.6230,
      estado: "Pendiente"
    },
    {
      titulo: "Poda de árbol urgente",
      categoria: "Poda o arbolado",
      lat: -34.6490,
      lng: -58.6120,
      estado: "Resuelto"
    }
  ];

  // 4. Recorrer el array y dibujar los pines/marcadores con emergentes
  reportes.forEach((rep) => {
    L.marker([rep.lat, rep.lng])
      .addTo(map)
      .bindPopup(`
        <div style="font-family: sans-serif; padding: 2px;">
          <h4 style="margin: 0 0 4px 0; color: #1a1a1a; font-size: 14px;">${rep.titulo}</h4>
          <p style="margin: 0; font-size: 12px; color: #555;"><strong>Categoría:</strong> ${rep.categoria}</p>
          <p style="margin: 2px 0 0 0; font-size: 12px; color: #d9381e;"><strong>Estado:</strong> ${rep.estado}</p>
        </div>
      `);
  });
});