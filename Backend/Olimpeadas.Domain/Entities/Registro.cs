using System;

namespace Olimpeadas.Domain.Entities
{
    // La restricción de unicidad (Reporte, Noticia) tambien se declara en el DbContext
    
    public class Registro
    {
        public int Id { get; set; }

        public int ReporteId { get; set; }

        public int NoticiaId { get; set; }

        public DateTime Fecha { get; set; } // Fecha de vinculación reporte-noticia

        public string? Comentario { get; set; }

        // Relaciones
        public Reporte Reporte { get; set; } = null!;
        public Noticia Noticia { get; set; } = null!;
    }
}
