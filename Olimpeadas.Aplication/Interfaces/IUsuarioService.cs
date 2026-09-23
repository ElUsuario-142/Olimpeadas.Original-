using Olimpeadas.Aplication.DTOs.Usuario;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO?> GetByIdAsync(int id);
        Task<IEnumerable<UsuarioDTO>> GetAllAsync();
        Task<IEnumerable<UsuarioDTO>> GetByMunicipioAsync(int municipioId);
        Task<IEnumerable<UsuarioDTO>> GetByRolAsync(Rol rol);
        Task<IEnumerable<UsuarioDTO>> GetActivosAsync();
        Task<UsuarioDTO> UpdateAsync(int id, ActualizarUsuarioDTO dto);
        Task<bool> CambiarEstadoActivoAsync(int id, bool activo);
    }
}
