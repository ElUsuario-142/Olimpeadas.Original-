using System;
using System.Collections.Generic;
using System.Text;

namespace Olimpeadas.Domain.Entities
{
    public class Direccion
    {
        public int Id { get; set; }
         public string? Calle { get; set; } // Dirección y altura
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }

        // Relación: una dirección puede aparecer en varios reportes (1:N)
        public ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    }
}
