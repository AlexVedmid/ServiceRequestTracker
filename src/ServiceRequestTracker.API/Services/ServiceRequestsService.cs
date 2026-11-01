using MassTransit;
using ServiceRequestTracker.API.Mapping;
using ServiceRequestTracker.API.Repositories;
using ServiceRequestTracker.API.Requests;
using ServiceRequestTracker.API.Responses;
using ServiceRequestTracker.Common.Enums;
using ServiceRequestTracker.Common.Models;
using ServiceRequestTracker.Communication;

namespace ServiceRequestTracker.API.Services;

public class ServiceRequestsService : IServiceRequestsService
{
    private readonly IServiceRequestsRepository _serviceRequestsRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    private readonly ServiceRequestsMapper _mapper = new();

    public ServiceRequestsService(IServiceRequestsRepository serviceRequestsRepository,
        IPublishEndpoint publishEndpoint)
    {
        _serviceRequestsRepository = serviceRequestsRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<GetServiceRequestResponse?> GetServiceRequest(Guid id)
    {
        var serviceRequest = await _serviceRequestsRepository.FindAsync(id);
        return _mapper.ToResponse(serviceRequest);
    }

    public async Task<SearchResult<GetServiceRequestResponse>> GetServiceRequests(GetServiceRequestsRequest requestsRequest, CancellationToken token)
    {
        var serviceRequests = await _serviceRequestsRepository.GetServiceRequests(requestsRequest, token);
        return new SearchResult<GetServiceRequestResponse>(
            [.. serviceRequests.Items.Select(_mapper.ToResponse)],
            serviceRequests.TotalCount
        );
    }

    public Task CreateServiceRequest(CreateServiceRequest request)
    {
        var message = _mapper.ToMessage(request);
        return _publishEndpoint.Publish(message);
    }

    public Task UpdateServiceRequest(Guid id, Status status)
    {
        var message = new UpdateServiceRequestMessage
        {
            Id = id,
            Status = status
        };
        return _publishEndpoint.Publish(message);
    }
}