using LearningDDD.Domain.Interfaces;
using LearningDDD.Domain.Models;
using LearningDDD.Infrastructure.Persistent;

namespace LearningDDD.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IRepository<Group>? _groups; // Nullable for lazy initialization

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        // Lazy initialization of repositories
        public IRepository<Group> Groups
        {
            get { return _groups ??= new Repository<Group>(_context); }
        }

        // This is where all tracked changes are committed atomically
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}