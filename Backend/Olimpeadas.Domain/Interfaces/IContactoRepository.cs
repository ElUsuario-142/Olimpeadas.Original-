using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Domain.Interfaces
{
    public interface IContactoRepository : IRepository<Contacto>
    {
        Task<IEnumerable<Contacto>> GetByMunicipioIdAsync(int municipioId);
        Task<IEnumerable<Contacto>> GetByTipoAsync(TipoEmergencia tipo);
    }
}
