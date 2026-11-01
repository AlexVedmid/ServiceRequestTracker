using MassTransit;
using ServiceRequestTracker.Communication;

namespace ServiceRequestTracker.Service.Consumers;

public class UpdateServiceRequestConsumer : IConsumer<UpdateServiceRequestMessage>
{
    public Task Consume(ConsumeContext<UpdateServiceRequestMessage> context)
    {
        throw new NotImplementedException();
    }
}