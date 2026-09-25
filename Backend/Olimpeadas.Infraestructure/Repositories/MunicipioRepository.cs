using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class MunicipioRepository : Repository<Municipio>, IMunicipioRepository
    {
        public MunicipioRepository(OlimpeadasDbContext context) : base(context) { }

        public async Task<IEnumerable<Municipio>> GetByLocalidadAsync(Localidad localidad)
        {
            return await _dbSet
                .Where(m => m.Localidad == localidad)
                .ToListAsync();
        }

        public async Task<Municipio?> GetWithUsuariosAsync(int id)
        {
            return await _dbSet
                .Include(m => m.Usuarios)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
