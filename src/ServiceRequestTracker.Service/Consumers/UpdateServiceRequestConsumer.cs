using MassTransit;
using ServiceRequestTracker.Communication;
using ServiceRequestTracker.Service.Repositories;

namespace ServiceRequestTracker.Service.Consumers;

public class UpdateServiceRequestConsumer : IConsumer<UpdateServiceRequestMessage>
{
    private readonly IServiceRequestsRepository _repository;

    public UpdateServiceRequestConsumer(IServiceRequestsRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<UpdateServiceRequestMessage> context)
    {
        var message = context.Message;
        var serviceRequest = await _repository.FindAsync(message.Id);

        serviceRequest.Status = message.NewStatus;
        await _repository.SaveChangesAsync();
    }
}