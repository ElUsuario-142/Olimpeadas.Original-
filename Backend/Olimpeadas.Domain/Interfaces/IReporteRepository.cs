using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Domain.Interfaces
{
    public interface IReporteRepository : IRepository<Reporte>
    {
        Task<Reporte?> GetWithDetallesAsync(int id);
        Task<IEnumerable<Reporte>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Reporte>> GetByEstadoAsync(Estado estado);
        Task<IEnumerable<Reporte>> GetByCategoriaIdAsync(int categoriaId);
        Task<IEnumerable<Reporte>> GetByEmpleadoIdAsync(int empleadoId);
        Task<IEnumerable<Reporte>> GetSinAsignarAsync();
    }
}
