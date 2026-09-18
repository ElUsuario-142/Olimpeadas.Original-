using System;
using System.Collections.Generic;
using System.Text;

namespace Olimpeadas.Domain.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }

        // Relación
        public ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();
    }
}
