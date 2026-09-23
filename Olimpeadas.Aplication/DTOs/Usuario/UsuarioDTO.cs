using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Usuario
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public int MunicipioId { get; set; }
        public string? MunicipioNombre { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string DNI { get; set; }
        public required string Telefono { get; set; }
        public required string Email { get; set; }
        public Rol Rol { get; set; }
        public bool Activo { get; set; }
    }
}
