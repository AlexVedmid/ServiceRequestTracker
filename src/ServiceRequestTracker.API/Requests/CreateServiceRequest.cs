using ServiceRequestTracker.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceRequestTracker.API.Requests;

public record CreateServiceRequest
{
    [MaxLength(200)]
    public string Title { get; set; }

    [MaxLength(2000)]
    public string Description { get; set; }

    public Priority Priority { get; set; }

    public Status Status { get; set; }
};