using MassTransit;
using ServiceRequestTracker.API.Mapping;
using ServiceRequestTracker.API.Repositories;
using ServiceRequestTracker.API.Requests;
using ServiceRequestTracker.API.Responses;
using ServiceRequestTracker.Common.Enums;
using ServiceRequestTracker.Common.Models;
using ServiceRequestTracker.Communication;
using ServiceRequestTracker.Persistence;

namespace ServiceRequestTracker.API.Services;

public class ServiceRequestsService : IServiceRequestsService
{
    private readonly IServiceRequestsRepository _serviceRequestsRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ServiceRequestTrackerDbContext _dbContext;
    private readonly ServiceRequestsMapper _mapper = new();

    public ServiceRequestsService(IServiceRequestsRepository serviceRequestsRepository,
        IPublishEndpoint publishEndpoint, ServiceRequestTrackerDbContext dbContext)
    {
        _serviceRequestsRepository = serviceRequestsRepository;
        _publishEndpoint = publishEndpoint;
        _dbContext = dbContext;
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

    public async Task CreateServiceRequest(CreateServiceRequest request)
    {
        var message = _mapper.ToMessage(request);

        await _publishEndpoint.Publish(message);
        // save message to outbox table
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateServiceRequest(Guid id, Status previousStatus, Status newStatus)
    {
        var message = new UpdateServiceRequestMessage
        {
            Id = id,
            PreviousStatus = previousStatus,
            NewStatus = newStatus
        };

        await _publishEndpoint.Publish(message);
        // save message to outbox table
        await _dbContext.SaveChangesAsync();
    }
}