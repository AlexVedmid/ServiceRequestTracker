using Microsoft.EntityFrameworkCore;
using ServiceRequestTracker.Common.BaseRequests;
using ServiceRequestTracker.Common.Models;

namespace ServiceRequestTracker.Persistence.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<SearchResult<T>> ToPagedListAsync<T>(this IQueryable<T> queryable,
            BaseRequestWithPagination request, CancellationToken cancellationToken)
        {
            if (request.PageSize <= 0 || request.Page <= 0)
            {
                return new SearchResult<T>([], 0);
            }

            var totalCount = await queryable.CountAsync(cancellationToken);
            var skipCount = request.PageSize * (request.Page - 1);

            if (skipCount > 0)
            {
                queryable = queryable.Skip(skipCount);
            }

            if (request.PageSize > 0)
            {
                queryable = queryable.Take(request.PageSize);
            }

            var list = await queryable.ToListAsync(cancellationToken);
            return new SearchResult<T>(list, totalCount);
        }
    }
}
