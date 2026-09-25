using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class NoticiaRepository : Repository<Noticia>, INoticiaRepository
    {
        public NoticiaRepository(OlimpeadasDbContext context) : base(context) { }

        public async Task<IEnumerable<Noticia>> GetByMunicipioIdAsync(int municipioId)
        {
            return await _dbSet
                .Where(n => n.MunicipioId == municipioId)
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();
        }

        public async Task<Noticia?> GetWithRegistrosAsync(int id)
        {
            return await _dbSet
                .Include(n => n.Registros)
                    .ThenInclude(r => r.Reporte)
                .Include(n => n.Municipio)
                .FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}
