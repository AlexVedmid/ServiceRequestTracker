using ServiceRequestTracker.Common.Enums;

namespace ServiceRequestTracker.Communication
{
    public class CreateServiceRequestMessage
    {
        public string Title { get; init; }

        public string Description { get; init; }

        public Priority Priority { get; init; }

        public Status Status { get; init; }
    }
}
