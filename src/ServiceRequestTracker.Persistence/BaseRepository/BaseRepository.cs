using Microsoft.EntityFrameworkCore;
using ServiceRequestTracker.Persistence.Entities;

namespace ServiceRequestTracker.Persistence.BaseRepository
{
    public class BaseRepository<T> : BaseReadOnlyRepository<T>, IBaseRepository<T> where T : BaseEntity, new()
    {
        protected BaseRepository(DbContext dbContext) : base(dbContext) {}

        public virtual async Task AddAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
        }

        public virtual Task AddRangeAsync(IEnumerable<T> entities)
        {
            return DbContext.Set<T>().AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            DbContext.Set<T>().Update(entity);
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }
    }
}
