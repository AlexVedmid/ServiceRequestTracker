using MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using ServiceRequestTracker.Common.Enums;
using ServiceRequestTracker.Communication;
using ServiceRequestTracker.Service.Mapping;
using ServiceRequestTracker.Service.Repositories;

namespace ServiceRequestTracker.Service.Consumers;

public class CreateServiceRequestConsumer : IConsumer<CreateServiceRequestMessage>
{
    private readonly IServiceRequestsRepository _repository;
    private readonly ILogger<CreateServiceRequestConsumer> _logger;
    private readonly ServiceRequestsMapper _mapper = new();

    public CreateServiceRequestConsumer(IServiceRequestsRepository repository,
        ILogger<CreateServiceRequestConsumer> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateServiceRequestMessage> context)
    {
        var message = context.Message;
        var serviceRequest = _mapper.ToEntity(message);

        await _repository.AddAsync(serviceRequest);
        await _repository.SaveChangesAsync();

        string logEntry;

        if (message.Priority == Priority.High)
        {
            logEntry = $"ServiceRequest \"{serviceRequest.Id} was created with priority {message.Priority}";
            _logger.LogInformation(logEntry);
        }

        if (message.Status == Status.Done)
        {
            logEntry = $"ServiceRequest \"{serviceRequest.Id} was created with status {message.Status}";
            _logger.LogInformation(logEntry);
        }
    }
}