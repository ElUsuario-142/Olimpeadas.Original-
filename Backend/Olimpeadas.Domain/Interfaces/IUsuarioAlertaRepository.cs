using Olimpeadas.Domain.Entities;

namespace Olimpeadas.Domain.Interfaces
{
    public interface IUsuarioAlertaRepository : IRepository<UsuarioAlerta>
    {
        Task<IEnumerable<UsuarioAlerta>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<UsuarioAlerta>> GetByAlertaIdAsync(int alertaId);
        Task<bool> ExisteVinculoAsync(int usuarioId, int alertaId);
    }
}
