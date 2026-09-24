using Olimpeadas.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace Olimpeadas.Aplication.DTOs.Municipio
{
    public class CrearMunicipioDTO
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
        public required string Nombre { get; set; }

        public required Localidad Localidad { get; set; }
    }
}
