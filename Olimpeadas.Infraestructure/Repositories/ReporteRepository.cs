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

        public async Task<IEnumerable<Reporte>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet
                .Include(r => r.Categoria)
                .Include(r => r.Direccion)
                .Where(r => r.UsuarioId == usuarioId)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reporte>> GetByEstadoAsync(Estado estado)
        {
            return await _dbSet
                .Include(r => r.Usuario)
                .Include(r => r.Categoria)
                .Where(r => r.Estado == estado)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reporte>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await _dbSet
                .Include(r => r.Usuario)
                .Where(r => r.CategoriaId == categoriaId)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reporte>> GetByEmpleadoIdAsync(int empleadoId)
        {
            return await _dbSet
                .Include(r => r.Usuario)
                .Include(r => r.Categoria)
                .Include(r => r.Direccion)
                .Where(r => r.EmpleadoEncargadoId == empleadoId)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reporte>> GetSinAsignarAsync()
        {
            return await _dbSet
                .Include(r => r.Usuario)
                .Include(r => r.Categoria)
                .Include(r => r.Direccion)
                .Where(r => r.EmpleadoEncargadoId == null)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }
    }
}
