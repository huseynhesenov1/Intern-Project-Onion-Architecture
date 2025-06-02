using Project.Domain.Entities.Commons;

namespace Project.Domain.Entities;

public class Order : BaseAuditableEntity
{
    public Product Product { get; set; }
    public int ProductId { get; set; }
    public int ProductCount { get; set; }
    public int WorkerId { get; set; }
    public Worker Worker { get; set; }
    public decimal TotalPrice { get; set; }
}
