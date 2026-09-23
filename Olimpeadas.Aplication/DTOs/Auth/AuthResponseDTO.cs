using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Rol Rol { get; set; }
        public int MunicipioId { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
