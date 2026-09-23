using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Usuario
{
    public class ActualizarUsuarioDTO
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public Rol? Rol { get; set; }
        public bool? Activo { get; set; }
        public int? MunicipioId { get; set; }
    }
}
