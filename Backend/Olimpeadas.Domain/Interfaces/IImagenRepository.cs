using Olimpeadas.Domain.Entities;

namespace Olimpeadas.Domain.Interfaces
{
    public interface IImagenRepository : IRepository<Imagen>
    {
        Task<IEnumerable<Imagen>> GetByReporteIdAsync(int reporteId);
    }
}
