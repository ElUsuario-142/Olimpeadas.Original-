using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Domain.Interfaces
{
    public interface IMunicipioRepository : IRepository<Municipio>
    {
        Task<IEnumerable<Municipio>> GetByLocalidadAsync(Localidad localidad);
        Task<Municipio?> GetWithUsuariosAsync(int id);
    }
}
