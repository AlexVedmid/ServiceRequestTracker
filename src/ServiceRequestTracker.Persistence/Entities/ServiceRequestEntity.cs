using ServiceRequestTracker.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceRequestTracker.Persistence.Entities
{
    [Table("ServiceRequests")]
    public class ServiceRequestEntity : BaseEntity
    {
        [Column(TypeName = "text")]
        [StringLength(200)]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        [StringLength(2000)]
        public string Description { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }
    }
}
