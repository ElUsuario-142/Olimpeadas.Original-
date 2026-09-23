using Olimpeadas.Aplication.DTOs.Alerta;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.Interfaces
{
    public interface IAlertaService
    {
        Task<AlertaDTO> CrearAsync(CrearAlertaDTO dto);
        Task<AlertaDTO?> GetByIdAsync(int id);
        Task<IEnumerable<AlertaDTO>> GetAllAsync();
        Task<IEnumerable<AlertaDTO>> GetByEstadoAsync(EstadoAlerta estado);
        Task<IEnumerable<AlertaDTO>> GetByTipoAsync(TipoEmergencia tipo);
        Task<AlertaDTO> CambiarEstadoAsync(int id, ActualizarAlertaDTO dto);
        Task<IEnumerable<AlertaDTO>> GetAlertasPorUsuarioAsync(int usuarioId);
    }
}
