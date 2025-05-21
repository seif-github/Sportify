using Microsoft.EntityFrameworkCore;
using sportify.DAL.Data;
using sportify.DAL.Repositories.Contracts;
using System.Linq.Expressions;

namespace sportify.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(List<T> entities) => await _dbSet.AddRangeAsync(entities);

        public void Update(T entity) => _dbSet.Update(entity);
        //public void Update(T entity)
        //{
        //    // If entity is already tracked, do not try to re-track it
        //    var trackedEntity = _context.ChangeTracker.Entries<T>()
        //        .FirstOrDefault(e => EqualityComparer<object>.Default.Equals(
        //            e.Property("Id").CurrentValue, entity.GetType().GetProperty("Id").GetValue(entity)));

        //    if (trackedEntity != null)
        //    {
        //        // Entity already tracked, apply property values
        //        _context.Entry(trackedEntity.Entity).CurrentValues.SetValues(entity);
        //    }
        //    else
        //    {
        //        // Entity not tracked, track it
        //        _dbSet.Update(entity);
        //    }
        //}

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null) _dbSet.Remove(entity);
        }

    }
}
