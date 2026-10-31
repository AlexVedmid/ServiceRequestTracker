using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiceRequestTracker.Common.BaseRequests
{
    public abstract record BaseRequestWithPagination
    {
        [DefaultValue(1)]
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [DefaultValue(10)]
        [Range(1, 1000)]
        public int PageSize { get; set; } = 10;
    }
}
