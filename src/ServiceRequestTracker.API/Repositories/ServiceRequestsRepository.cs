using Microsoft.EntityFrameworkCore;
using ServiceRequestTracker.API.Requests;
using ServiceRequestTracker.Common.Models;
using ServiceRequestTracker.Persistence;
using ServiceRequestTracker.Persistence.BaseRepository;
using ServiceRequestTracker.Persistence.Entities;
using ServiceRequestTracker.Persistence.Extensions;

namespace ServiceRequestTracker.API.Repositories;

public class ServiceRequestsRepository : BaseReadOnlyRepository<ServiceRequestEntity>, IServiceRequestsRepository
{
    public ServiceRequestsRepository(ServiceRequestTrackerDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<SearchResult<ServiceRequestEntity>> GetServiceRequests(GetServiceRequestsRequest request, CancellationToken token)
    {
        var query = DbContext.Set<ServiceRequestEntity>().AsNoTracking();

        if (request.Status.HasValue)
        {
            query = query.Where(d => d.Status == request.Status.Value);
        }

        query = query.OrderByDescending(d => d.CreatedAt);

        var pagedResult = await query.ToPagedListAsync(request, token);

        return pagedResult;
    }
}