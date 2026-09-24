using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    public class ReporteRepository : Repository<Reporte>, IReporteRepository
    {
        public ReporteRepository(OlimpeadasDbContext context) : base(context) { }

        private IQueryable<Reporte> ListadoBase() => _dbSet
           .AsNoTracking()
           .Include(r => r.Usuario)
           .Include(r => r.Categoria)
           .Include(r => r.Direccion)
           .Include(r => r.EmpleadoEncargado)
           .Include(r => r.Imagenes)
           .OrderByDescending(r => r.FechaCreacion);

        public async Task<Reporte?> GetWithDetallesAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Usuario)
                .Include(r => r.Categoria)
                .Include(r => r.Direccion)
                .Include(r => r.EmpleadoEncargado)
                .Include(r => r.Imagenes)
                .Include(r => r.HistorialesEstado)
                .FirstOrDefaultAsync(r => r.Id == id);
        }


        public override async Task<IEnumerable<Reporte>> GetAllAsync()
        {
            return await ListadoBase().ToListAsync();
        }

        public async Task<IEnumerable<Reporte>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await ListadoBase().Where(r => r.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task<IEnumerable<Reporte>> GetByEstadoAsync(Estado estado)
        {
            return await ListadoBase().Where(r => r.Estado == estado).ToListAsync();
        }

        public async Task<IEnumerable<Reporte>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await ListadoBase().Where(r => r.CategoriaId == categoriaId).ToListAsync();
        }

        public async Task<IEnumerable<Reporte>> GetByEmpleadoIdAsync(int empleadoId)
        {
            return await ListadoBase().Where(r => r.EmpleadoEncargadoId == empleadoId).ToListAsync();
        }

        public async Task<IEnumerable<Reporte>> GetSinAsignarAsync()
        {
            return await ListadoBase().Where(r => r.EmpleadoEncargadoId == null).ToListAsync();
        }
    }
}
