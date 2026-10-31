using ServiceRequestTracker.API.Requests;
using ServiceRequestTracker.Common.Models;
using ServiceRequestTracker.Persistence.BaseRepository;
using ServiceRequestTracker.Persistence.Entities;

namespace ServiceRequestTracker.API.Repositories;

public interface IServiceRequestsRepository : IBaseReadOnlyRepository<ServiceRequestEntity>
{
    Task<SearchResult<ServiceRequestEntity>> GetServiceRequests(
        GetServiceRequestsRequest request, CancellationToken token);
}