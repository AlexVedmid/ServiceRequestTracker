using MassTransit;
using ServiceRequestTracker.Common.Enums;
using ServiceRequestTracker.Communication;
using ServiceRequestTracker.Service.Repositories;

namespace ServiceRequestTracker.Service.Consumers;

public class UpdateServiceRequestConsumer : IConsumer<UpdateServiceRequestMessage>
{
    private readonly IServiceRequestsRepository _repository;
    private readonly ILogger<UpdateServiceRequestConsumer> _logger;

    public UpdateServiceRequestConsumer(IServiceRequestsRepository repository,
        ILogger<UpdateServiceRequestConsumer> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UpdateServiceRequestMessage> context)
    {
        var message = context.Message;
        var serviceRequest = await _repository.FindAsync(message.Id);

        serviceRequest.Status = message.NewStatus;
        await _repository.SaveChangesAsync();

        var logEntry = $"ServiceRequest {message.Id} changed its status from {message.PreviousStatus} to {message.NewStatus}";
        if (message.NewStatus == Status.Done)
        {
            _logger.LogInformation(logEntry);
        }
    }
}