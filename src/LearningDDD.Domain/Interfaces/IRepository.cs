using System.Linq.Expressions;

namespace LearningDDD.Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task<T?> FindByIdAsync(Guid id);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? includes = null);
        Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> include);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
