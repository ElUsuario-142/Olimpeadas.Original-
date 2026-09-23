using Olimpeadas.Domain.Entities;

namespace Olimpeadas.Domain.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<Categoria?> GetByNombreAsync(string nombre);
    }
}
