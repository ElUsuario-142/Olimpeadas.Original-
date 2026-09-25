using System.ComponentModel.DataAnnotations;

namespace Olimpeadas.Aplication.DTOs.Reporte

{
    public class CrearReporteDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
        public int CategoriaId { get; set; }

        [StringLength(100, ErrorMessage = "La calle no puede superar los 100 caracteres.")]
        public string? Calle { get; set; }

        [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90.")]
        public decimal Latitud { get; set; }

        [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180.")]
        public decimal Longitud { get; set; }

        // 50 = HasMaxLength(50) del DbContext  por que si no un titulo largo petaria la base (500)
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 50 caracteres.")]
        public required string Titulo { get; set; }

        // La columna es Text, sin tope en la base 2000 esta bien
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 2000 caracteres.")]
        public required string Descripcion { get; set; }

        [MaxLength(5, ErrorMessage = "Se permiten hasta 5 imágenes por reporte.")]
        public List<string>? ImagenesUrls { get; set; }
    }
}
