using ServiceRequestTracker.Common.Enums;

namespace ServiceRequestTracker.Communication
{
    public class UpdateServiceRequestMessage
    {
        public Guid Id { get; init; }

        public Status Status { get; init; }
    }
}
