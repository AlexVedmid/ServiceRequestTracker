using ServiceRequestTracker.API.Requests;
using ServiceRequestTracker.API.Responses;
using ServiceRequestTracker.Common.Models;

namespace ServiceRequestTracker.API.Services;

public interface IServiceRequestsService
{
    Task<GetServiceRequestResponse?> GetServiceRequest(Guid id);

    Task<SearchResult<GetServiceRequestResponse>> GetServiceRequests(
        GetServiceRequestsRequest requestsRequest, CancellationToken token);

    Task CreateServiceRequest(CreateServiceRequest request);
}