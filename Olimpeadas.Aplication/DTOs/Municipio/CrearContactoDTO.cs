using Olimpeadas.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Olimpeadas.Aplication.DTOs.Municipio
{
    public class CrearContactoDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un municipio válido.")]
        public int MunicipioId { get; set; }

        public required TipoEmergencia Tipo { get; set; }

        [RegularExpression(@"^[0-9+\-\s()]{3,20}$",
           ErrorMessage = "El número debe tener entre 3 y 20 caracteres (dígitos, +, -, espacios o paréntesis).")]
        public required string Numero { get; set; }
    }
}
