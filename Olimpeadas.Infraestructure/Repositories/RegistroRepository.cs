using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class RegistroRepository : Repository<Registro>, IRegistroRepository
    {
        public RegistroRepository(OlimpeadasDbContext context) : base(context) { }

        public async Task<IEnumerable<Registro>> GetByReporteIdAsync(int reporteId)
        {
            return await _dbSet
                .Include(r => r.Noticia)
                .Where(r => r.ReporteId == reporteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registro>> GetByNoticiaIdAsync(int noticiaId)
        {
            return await _dbSet
                .Include(r => r.Reporte)
                .Where(r => r.NoticiaId == noticiaId)
                .ToListAsync();
        }

        public async Task<bool> ExisteVinculoAsync(int reporteId, int noticiaId)
        {
            return await _dbSet
                .AnyAsync(r => r.ReporteId == reporteId && r.NoticiaId == noticiaId);
        }
    }
}
