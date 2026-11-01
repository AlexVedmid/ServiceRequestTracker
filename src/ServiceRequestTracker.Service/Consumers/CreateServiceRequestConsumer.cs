using MassTransit;
using ServiceRequestTracker.Communication;

namespace ServiceRequestTracker.Service.Consumers;

public class CreateServiceRequestConsumer : IConsumer<CreateServiceRequestMessage>
{
    public Task Consume(ConsumeContext<CreateServiceRequestMessage> context)
    {
        throw new NotImplementedException();
    }
}