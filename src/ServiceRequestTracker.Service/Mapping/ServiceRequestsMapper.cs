using Riok.Mapperly.Abstractions;
using ServiceRequestTracker.Communication;
using ServiceRequestTracker.Persistence.Entities;

namespace ServiceRequestTracker.Service.Mapping;

[Mapper]
public partial class ServiceRequestsMapper
{
    public partial ServiceRequestEntity ToEntity(CreateServiceRequestMessage request);
}