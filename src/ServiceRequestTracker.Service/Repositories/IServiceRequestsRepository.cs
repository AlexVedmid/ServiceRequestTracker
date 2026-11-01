using ServiceRequestTracker.Persistence.BaseRepository;
using ServiceRequestTracker.Persistence.Entities;

namespace ServiceRequestTracker.Service.Repositories;

public interface IServiceRequestsRepository : IBaseRepository<ServiceRequestEntity>
{
}