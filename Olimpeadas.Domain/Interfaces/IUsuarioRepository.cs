using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetByDNIAsync(string dni);
        Task<IEnumerable<Usuario>> GetByMunicipioIdAsync(int municipioId);
        Task<IEnumerable<Usuario>> GetByRolAsync(Rol rol);
        Task<IEnumerable<Usuario>> GetActivosAsync();
    }
}
