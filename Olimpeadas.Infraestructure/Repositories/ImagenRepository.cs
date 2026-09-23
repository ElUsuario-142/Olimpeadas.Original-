using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class ImagenRepository : Repository<Imagen>, IImagenRepository
    {
        public ImagenRepository(OlimpeadasDbContext context) : base(context) { }

        public async Task<IEnumerable<Imagen>> GetByReporteIdAsync(int reporteId)
        {
            return await _dbSet
                .Where(i => i.ReporteId == reporteId)
                .ToListAsync();
        }
    }
}
