using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(OlimpeadasDbContext context) : base(context) { }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Municipio)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario?> GetByDNIAsync(string dni)
        {
            return await _dbSet
                .Include(u => u.Municipio)
                .FirstOrDefaultAsync(u => u.DNI == dni);
        }

        public async Task<IEnumerable<Usuario>> GetByMunicipioIdAsync(int municipioId)
        {
            return await _dbSet
                .Where(u => u.MunicipioId == municipioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetByRolAsync(Rol rol)
        {
            return await _dbSet
                .Where(u => u.Rol == rol)
                .ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetActivosAsync()
        {
            return await _dbSet
                .Where(u => u.Activo)
                .ToListAsync();
        }
    }
}
