using ServiceRequestTracker.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceRequestTracker.API.Requests;

public record CreateServiceRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; }

    [Required]
    public Priority Priority { get; set; } = Priority.Normal;

    public static Status Status => Status.New;
};