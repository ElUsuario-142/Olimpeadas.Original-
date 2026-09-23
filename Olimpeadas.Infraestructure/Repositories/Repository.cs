using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;

namespace Olimpeadas.Infraestructure.Repositories
{
    /// <summary>
    /// Implementación genérica base del patrón Repository.
    /// Todas las operaciones CRUD comunes se resuelven acá.
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly OlimpeadasDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(OlimpeadasDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
