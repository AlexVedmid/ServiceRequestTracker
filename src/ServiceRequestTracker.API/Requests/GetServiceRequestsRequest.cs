using ServiceRequestTracker.Common.BaseRequests;
using ServiceRequestTracker.Common.Enums;

namespace ServiceRequestTracker.API.Requests;

public record GetServiceRequestsRequest : BaseRequestWithPagination
{
    public Status? Status { get; set; }
};