using System.Linq.Expressions;
using ServiceRequestTracker.Persistence.Entities;

namespace ServiceRequestTracker.Persistence.BaseRepository
{
    public interface IBaseReadOnlyRepository<T> where T : BaseEntity, new()
    {
        Task<T> FindAsync(Guid id, bool reload = false);

        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);
    }
}
