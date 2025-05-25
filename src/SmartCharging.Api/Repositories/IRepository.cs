using SmartCharging.Api.Models;
using System.Linq.Expressions;

namespace SmartCharging.Api.Repositories
{
    public interface IRepository<T>
    {
        Task<T?> FindByIdAsync(Guid id);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? includes = null);
        Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> include);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
