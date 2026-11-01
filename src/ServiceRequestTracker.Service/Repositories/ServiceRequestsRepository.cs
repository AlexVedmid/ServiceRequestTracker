using ServiceRequestTracker.Persistence;
using ServiceRequestTracker.Persistence.BaseRepository;
using ServiceRequestTracker.Persistence.Entities;

namespace ServiceRequestTracker.Service.Repositories;

public class ServiceRequestsRepository : BaseRepository<ServiceRequestEntity>, IServiceRequestsRepository
{
    public ServiceRequestsRepository(ServiceRequestTrackerDbContext dbContext) : base(dbContext)
    {
    }
}