using Olimpeadas.Aplication.DTOs.Auth;
using Olimpeadas.Aplication.DTOs.Usuario;

namespace Olimpeadas.Aplication.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto);
        Task<AuthResponseDTO> RegisterAsync(CrearUsuarioDTO registroDto);
    }
}
