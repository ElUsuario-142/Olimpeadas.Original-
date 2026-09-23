using Olimpeadas.Domain.Entities;

namespace Olimpeadas.Domain.Interfaces
{
    public interface IHistorialEstadoRepository : IRepository<HistorialEstado>
    {
        Task<IEnumerable<HistorialEstado>> GetByReporteIdAsync(int reporteId);
        Task<IEnumerable<HistorialEstado>> GetByUsuarioIdAsync(int usuarioId);
    }
}
