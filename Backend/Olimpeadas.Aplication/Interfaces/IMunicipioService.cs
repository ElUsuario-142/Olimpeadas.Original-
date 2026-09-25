using Olimpeadas.Aplication.DTOs.Municipio;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.Interfaces
{
    public interface IMunicipioService
    {
        Task<IEnumerable<MunicipioDTO>> GetAllMunicipiosAsync();
        Task<MunicipioDTO?> GetMunicipioByIdAsync(int id);
        Task<MunicipioDTO> CrearMunicipioAsync(CrearMunicipioDTO dto);
        Task<IEnumerable<ContactoDTO>> GetContactosByMunicipioAsync(int municipioId);
        Task<IEnumerable<ContactoDTO>> GetContactosByTipoAsync(TipoEmergencia tipo);
        Task<ContactoDTO> CrearContactoAsync(CrearContactoDTO dto);
    }
}
