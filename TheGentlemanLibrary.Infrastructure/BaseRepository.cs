using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TheGentlemanLibrary.Infrastructure
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(CancellationToken token) => await _dbSet.ToListAsync(token);

        public async Task<T> GetAsync(CancellationToken token, params object[] key) => await _dbSet.FindAsync(key, token);

        public async Task AddAsync(T entity, CancellationToken token)
        {
            await _dbSet.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(T entity, CancellationToken token)
        {
            if (entity == null)
                return;

            _dbSet.Update(entity);

            await _context.SaveChangesAsync(token);
        }

        public async Task RemoveAsync(CancellationToken token, params object[] key)
        {
            var entity = await GetAsync(token, key);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(token);
        }

        public async Task RemoveAsync( T entity, CancellationToken token)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(token);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken token) => _dbSet.AnyAsync(predicate, token);
    }
}
