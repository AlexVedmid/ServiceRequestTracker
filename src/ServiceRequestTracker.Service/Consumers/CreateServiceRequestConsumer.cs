using MassTransit;
using ServiceRequestTracker.Communication;
using ServiceRequestTracker.Service.Mapping;
using ServiceRequestTracker.Service.Repositories;

namespace ServiceRequestTracker.Service.Consumers;

public class CreateServiceRequestConsumer : IConsumer<CreateServiceRequestMessage>
{
    private readonly IServiceRequestsRepository _repository;
    private readonly ServiceRequestsMapper _mapper = new();

    public CreateServiceRequestConsumer(IServiceRequestsRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<CreateServiceRequestMessage> context)
    {
        var message = context.Message;
        var serviceRequest = _mapper.ToEntity(message);

        await _repository.AddAsync(serviceRequest);
        await _repository.SaveChangesAsync();
    }
}