using System.ComponentModel.DataAnnotations;
 
namespace Olimpeadas.Aplication.DTOs.Reporte
{
    public class AgregarImagenDTO
    {
        // El formato (http/https) se valida en el controller; acá el largo de la columna
        [StringLength(255, ErrorMessage = "La URL no puede superar los 255 caracteres.")]
        public required string Url { get; set; }
    }
}