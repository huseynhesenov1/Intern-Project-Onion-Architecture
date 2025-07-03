namespace Project.Domain.Entities.Commons;


public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
