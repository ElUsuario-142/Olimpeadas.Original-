using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(OlimpeadasDbContext context) : base(context) { }

        public async Task<Categoria?> GetByNombreAsync(string nombre)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Nombre == nombre);
        }
    }
}
