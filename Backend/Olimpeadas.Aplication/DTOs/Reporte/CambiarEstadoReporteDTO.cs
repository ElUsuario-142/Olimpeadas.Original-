using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Reporte
{
    public class CambiarEstadoReporteDTO
    {
        public Estado NuevoEstado { get; set; }
        public string? Comentario { get; set; }
    }
}
