using LearningDDD.Domain.Models;

namespace LearningDDD.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Expose repositories for Aggregate Roots
        IRepository<Group> Groups { get; }

        // The method to commit all changes tracked by the DbContext within this UoW
        Task<int> SaveChangesAsync();

        // Optional: If we ever need explicit transaction control beyond DbContext's default
        // Task BeginTransactionAsync();
        // Task CommitTransactionAsync();
        // Task RollbackTransactionAsync();
    }
}
