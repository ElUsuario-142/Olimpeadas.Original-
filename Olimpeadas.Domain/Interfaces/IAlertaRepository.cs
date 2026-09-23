using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Domain.Interfaces
{
    public interface IAlertaRepository : IRepository<Alerta>
    {
        Task<IEnumerable<Alerta>> GetByEstadoAsync(EstadoAlerta estado);
        Task<IEnumerable<Alerta>> GetByTipoAsync(TipoEmergencia tipo);
        Task<Alerta?> GetWithUsuariosAsync(int id);
    }
}
