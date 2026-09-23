using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Usuario
{
    public class CrearUsuarioDTO
    {
        public int MunicipioId { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string DNI { get; set; }
        public required string Telefono { get; set; }
        public required string Email { get; set; }
        public required string Contraseña { get; set; }
        public Rol Rol { get; set; } = Rol.Ciudadano;
    }
}
