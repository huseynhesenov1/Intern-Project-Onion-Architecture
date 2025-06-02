using Project.Domain.Entities.Commons;

namespace Project.Domain.Entities;

public class ProductDistrictPrice : BaseAuditableEntity
{
    public Product Product { get; set; }
    public int ProductId { get; set; }
    public District District { get; set; }
    public int DistrictId { get; set; }
    public decimal Price { get; set; }
}
