using Project.Domain.Entities.Commons;

namespace Project.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public Campaign? Campaign { get; set; }
    public int? CampaignId { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<ProductDistrictPrice> ProductDistrictPrices { get; set; }
}
