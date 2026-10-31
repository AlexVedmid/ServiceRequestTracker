using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServiceRequestTracker.Persistence.Entities;

namespace ServiceRequestTracker.Persistence.BaseRepository
{
    public class BaseReadOnlyRepository<T> : IBaseReadOnlyRepository<T> where T : BaseEntity, new()
    {
        protected BaseReadOnlyRepository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected DbContext DbContext { get; }

        public virtual async Task<T> FindAsync(Guid id, bool reload = false)
        {
            var entity = await DbContext.Set<T>().FindAsync(id);

            if (reload)
            {
                await DbContext.Set<T>().Entry(entity).ReloadAsync();
            }

            return entity;
        }

        public Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate).ToListAsync();
        }
    }
}
