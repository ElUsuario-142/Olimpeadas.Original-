using Olimpeadas.Aplication.DTOs.Noticia;

namespace Olimpeadas.Aplication.Interfaces
{
    public interface INoticiaService
    {
        Task<NoticiaDTO> CrearAsync(CrearNoticiaDTO dto);
        Task<NoticiaDTO?> GetByIdAsync(int id);
        Task<IEnumerable<NoticiaDTO>> GetAllAsync();
        Task<IEnumerable<NoticiaDTO>> GetByMunicipioAsync(int municipioId);
        Task<RegistroDTO> VincularReporteAsync(int noticiaId, VincularReporteDTO dto);
        Task<bool> DesvincularReporteAsync(int noticiaId, int reporteId);
    }
}
