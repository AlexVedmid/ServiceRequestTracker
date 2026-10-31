using ServiceRequestTracker.Persistence.Entities;

namespace ServiceRequestTracker.Persistence.BaseRepository
{
    public interface IBaseRepository<T> : IBaseReadOnlyRepository<T> where T : BaseEntity, new()
    {
        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        Task<int> SaveChangesAsync();
    }
}
