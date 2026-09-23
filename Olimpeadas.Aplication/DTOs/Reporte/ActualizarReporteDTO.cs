using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Reporte
{
    public class ActualizarReporteDTO
    {
        public Estado? Estado { get; set; }
        public int? EmpleadoEncargadoId { get; set; }
        public string? Comentario { get; set; }
        public int? UsuarioModificadorId { get; set; }
    }
}
