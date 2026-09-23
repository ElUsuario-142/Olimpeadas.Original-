using Olimpeadas.Domain.Entities;

namespace Olimpeadas.Domain.Interfaces
{
    public interface INoticiaRepository : IRepository<Noticia>
    {
        Task<IEnumerable<Noticia>> GetByMunicipioIdAsync(int municipioId);
        Task<Noticia?> GetWithRegistrosAsync(int id);
    }
}
