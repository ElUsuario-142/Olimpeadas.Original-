using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Reporte
{
    public class HistorialEstadoDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public Estado? EstadoAnterior { get; set; }
        public Estado EstadoNuevo { get; set; }
        public string? Comentario { get; set; }
        public DateTime Fecha { get; set; }
    }
}
