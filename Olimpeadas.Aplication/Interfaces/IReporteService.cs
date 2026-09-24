using Olimpeadas.Aplication.DTOs.Reporte;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.Interfaces
{
    public interface IReporteService
    {
        Task<ReporteDTO> CrearAsync(CrearReporteDTO dto, int usuarioId);

        Task<ReporteDTO?> GetByIdAsync(int id);

        Task<IEnumerable<ReporteDTO>> GetAllAsync();

        Task<IEnumerable<ReporteDTO>> GetByUsuarioAsync(int usuarioId);
       
        Task<IEnumerable<ReporteDTO>> GetByEstadoAsync(Estado estado);
        
        Task<IEnumerable<ReporteDTO>> GetByCategoriaAsync(int categoriaId);

        Task<IEnumerable<ReporteDTO>> GetByEmpleadoAsync(int empleadoId);
       
        Task<IEnumerable<ReporteDTO>> GetSinAsignarAsync();
        
        Task<ReporteDTO> AsignarEmpleadoAsync(int reporteId, AsignarEmpleadoDTO dto, int asignadorUsuarioId);
        
        Task<ReporteDTO> CambiarEstadoAsync(int reporteId, CambiarEstadoReporteDTO dto, int usuarioId);
        
        Task<bool> AgregarImagenAsync(int reporteId, string url);
        
        Task<IEnumerable<HistorialEstadoDTO>> GetHistorialAsync(int reporteId);
    }
}
