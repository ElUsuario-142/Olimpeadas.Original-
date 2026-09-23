using Olimpeadas.Domain.Entities;

namespace Olimpeadas.Domain.Interfaces
{
    public interface IDireccionRepository : IRepository<Direccion>
    {
        Task<Direccion?> GetByCoordenadasAsync(decimal latitud, decimal longitud);
    }
}
