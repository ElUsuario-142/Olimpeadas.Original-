using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class HistorialEstadoRepository : Repository<HistorialEstado>, IHistorialEstadoRepository
    {
        public HistorialEstadoRepository(OlimpeadasDbContext context) : base(context) { }

        public async Task<IEnumerable<HistorialEstado>> GetByReporteIdAsync(int reporteId)
        {
            return await _dbSet
                .Include(h => h.Usuario)
                .Where(h => h.ReporteId == reporteId)
                .OrderByDescending(h => h.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistorialEstado>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet
                .Include(h => h.Reporte)
                .Where(h => h.UsuarioId == usuarioId)
                .OrderByDescending(h => h.Fecha)
                .ToListAsync();
        }
    }
}
