using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class AlertaRepository : Repository<Alerta>, IAlertaRepository
    {
        public AlertaRepository(OlimpeadasDbContext context) : base(context) { }

        public async Task<IEnumerable<Alerta>> GetByEstadoAsync(EstadoAlerta estado)
        {
            return await _dbSet
                .Where(a => a.Estado == estado)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Alerta>> GetByTipoAsync(TipoEmergencia tipo)
        {
            return await _dbSet
                .Where(a => a.Tipo == tipo)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<Alerta?> GetWithUsuariosAsync(int id)
        {
            return await _dbSet
                .Include(a => a.UsuariosAlertas)
                    .ThenInclude(ua => ua.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
