using Riok.Mapperly.Abstractions;
using ServiceRequestTracker.API.Requests;
using ServiceRequestTracker.API.Responses;
using ServiceRequestTracker.Communication;
using ServiceRequestTracker.Persistence.Entities;

namespace ServiceRequestTracker.API.Mapping;

[Mapper]
public partial class ServiceRequestsMapper
{
    public partial GetServiceRequestResponse? ToResponse(ServiceRequestEntity? entity);

    public partial ServiceRequestEntity ToEntity(CreateServiceRequest request);

    public partial CreateServiceRequestMessage ToMessage(CreateServiceRequest request);
}