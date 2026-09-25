using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Reporte
{
    public class ReporteDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public int CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }
        public int DireccionId { get; set; }
        public string? DireccionCalle { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int? EmpleadoEncargadoId { get; set; }
        public string? EmpleadoEncargadoNombre { get; set; }
        public required string Titulo { get; set; }
        public required string Descripcion { get; set; }
        public Estado Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public List<string> ImagenesUrls { get; set; } = new();
        public List<HistorialEstadoDTO> HistorialEstados { get; set; } = new();
    }
}
