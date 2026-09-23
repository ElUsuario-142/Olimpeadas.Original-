using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class UsuarioAlertaRepository : Repository<UsuarioAlerta>, IUsuarioAlertaRepository
    {
        public UsuarioAlertaRepository(OlimpeadasDbContext context) : base(context) { }

        public async Task<IEnumerable<UsuarioAlerta>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet
                .Include(ua => ua.Alerta)
                .Where(ua => ua.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UsuarioAlerta>> GetByAlertaIdAsync(int alertaId)
        {
            return await _dbSet
                .Include(ua => ua.Usuario)
                .Where(ua => ua.AlertaId == alertaId)
                .ToListAsync();
        }

        public async Task<bool> ExisteVinculoAsync(int usuarioId, int alertaId)
        {
            return await _dbSet
                .AnyAsync(ua => ua.UsuarioId == usuarioId && ua.AlertaId == alertaId);
        }
    }
}
