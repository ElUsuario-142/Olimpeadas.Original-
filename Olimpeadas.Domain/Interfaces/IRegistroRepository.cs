using Olimpeadas.Domain.Entities;

namespace Olimpeadas.Domain.Interfaces
{
    public interface IRegistroRepository : IRepository<Registro>
    {
        Task<IEnumerable<Registro>> GetByReporteIdAsync(int reporteId);
        Task<IEnumerable<Registro>> GetByNoticiaIdAsync(int noticiaId);
        Task<bool> ExisteVinculoAsync(int reporteId, int noticiaId);
    }
}
