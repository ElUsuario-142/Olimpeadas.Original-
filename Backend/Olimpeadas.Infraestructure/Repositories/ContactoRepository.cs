using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class ContactoRepository : Repository<Contacto>, IContactoRepository
    {
        public ContactoRepository(OlimpeadasDbContext context) : base(context) { }

        public async Task<IEnumerable<Contacto>> GetByMunicipioIdAsync(int municipioId)
        {
            return await _dbSet
                .Where(c => c.MunicipioId == municipioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contacto>> GetByTipoAsync(TipoEmergencia tipo)
        {
            return await _dbSet
                .Include(c => c.Municipio)
                .Where(c => c.Tipo == tipo)
                .ToListAsync();
        }
    }
}
