using System.ComponentModel.DataAnnotations;
namespace Olimpeadas.Aplication.DTOs.Noticia
{
    public class CrearNoticiaDTO
    {

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un municipio válido.")]
        public int MunicipioId { get; set; }

        [StringLength(150, MinimumLength = 5, ErrorMessage = "El título debe tener entre 5 y 150 caracteres.")]
        public required string Titulo { get; set; }

        [StringLength(150, MinimumLength = 5, ErrorMessage = "El contenido debe tener entre 5 y 150 caracteres.")]
        public string? Contenido { get; set; }

        [MaxLength(50, ErrorMessage = "Se pueden vincular hasta 50 reportes al crear la noticia.")]
        public List<int>? ReportesIds { get; set; }
    }

    public class VincularReporteDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un reporte válido.")]
        public int ReporteId { get; set; }

        [StringLength(500, ErrorMessage = "El comentario no puede superar los 500 caracteres.")]
        public string? Comentario { get; set; }
    }
}
