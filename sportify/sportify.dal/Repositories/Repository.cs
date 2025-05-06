using Microsoft.EntityFrameworkCore;
using Sportify.DAL.Data;

namespace sportify.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbset;

        public Repository(ApplicationDbContext _context)
        {
            this._context = _context;
            this._dbset = _context.Set<T>();

        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> DeleteAsync(int id)
        {
            var e = await _dbset.FindAsync(id);
            if (e != null)
            {
                return null;
            }
            _dbset.Remove(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<List<T>> GetAllAsync()
        {
            List<T> All = await _dbset.ToListAsync();
            return All;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var Entity = await _dbset.FindAsync(id);
            return Entity;
        }

        public async Task<T> UpdateAsync(T Entity)
        {
            _dbset.Update(Entity);
            await _context.SaveChangesAsync();
            return Entity;
        }
    }
}
