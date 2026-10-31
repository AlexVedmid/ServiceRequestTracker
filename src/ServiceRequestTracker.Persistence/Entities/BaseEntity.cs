using SecurityDriven;

namespace ServiceRequestTracker.Persistence.Entities;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = FastGuid.NewPostgreSqlGuid();
    }

    protected BaseEntity(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets or sets the record identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the record created date.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the record modification date.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }
}
