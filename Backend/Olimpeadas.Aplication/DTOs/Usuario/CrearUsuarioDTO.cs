using Olimpeadas.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Olimpeadas.Aplication.DTOs.Usuario
{
    public class CrearUsuarioDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un municipio válido.")]
        public int MunicipioId { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 20 caracteres.")]
        public required string Nombre { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 20 caracteres.")]
        public required string Apellido { get; set; }

        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El DNI debe tener 7 u 8 dígitos, sin puntos.")]
        public required string DNI { get; set; }

        [StringLength(14, MinimumLength = 6, ErrorMessage = "El teléfono debe tener entre 6 y 14 caracteres.")]
        public required string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [StringLength(255)]
        public required string Email { get; set; }

        [StringLength(72, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 72 caracteres.")]
        public required string Contraseña { get; set; }

        public Rol Rol { get; set; } = Rol.Ciudadano; 
    }
}
