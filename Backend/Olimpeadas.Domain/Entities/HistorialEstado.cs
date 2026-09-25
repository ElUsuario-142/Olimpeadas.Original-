using System;
using System.Collections.Generic;
using System.Text;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Domain.Entities
{
    public class HistorialEstado
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; } // Quién realizó el cambio
        public int ReporteId { get; set; } // Reporte que cambio de estado
        public Estado? EstadoAnterior { get; set; } // null en el primer cambio
        public Estado EstadoNuevo { get; set; }
        public string? Comentario { get; set; }
        public DateTime Fecha { get; set; }

        // Relaciones
        public Usuario Usuario { get; set; } = null!;
        public Reporte Reporte { get; set; } = null!;
    }
}
