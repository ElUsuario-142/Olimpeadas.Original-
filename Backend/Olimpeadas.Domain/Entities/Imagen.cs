using System;
using System.Collections.Generic;
using System.Text;

namespace Olimpeadas.Domain.Entities
{
    public class Imagen
    {
        public int Id { get; set; }
        public int ReporteId { get; set; }
        public required string Url { get; set; }

        // Relación
        public Reporte Reporte { get; set; } = null!;
    }
}
