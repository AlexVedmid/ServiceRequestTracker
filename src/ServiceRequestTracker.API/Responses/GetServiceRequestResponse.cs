using ServiceRequestTracker.Common.Enums;

namespace ServiceRequestTracker.API.Responses;

public record GetServiceRequestResponse
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public Priority Priority { get; set; }

    public Status Status { get; set; }
};