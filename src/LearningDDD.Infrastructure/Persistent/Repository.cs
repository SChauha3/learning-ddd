using LearningDDD.Domain.Interfaces;
using LearningDDD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LearningDDD.Infrastructure.Persistent
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<T>();
        }

        public async Task<T?> FindByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                query = includes(query);
            }

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> include)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _appDbContext.Remove(entity);
        }
    }
}