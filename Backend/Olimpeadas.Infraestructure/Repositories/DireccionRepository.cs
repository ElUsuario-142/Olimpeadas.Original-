using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class DireccionRepository : Repository<Direccion>, IDireccionRepository
    {
        public DireccionRepository(OlimpeadasDbContext context) : base(context) { }

        public async Task<Direccion?> GetByCoordenadasAsync(decimal latitud, decimal longitud)
        {
            return await _dbSet
                .FirstOrDefaultAsync(d => d.Latitud == latitud && d.Longitud == longitud);
        }
    }
}
