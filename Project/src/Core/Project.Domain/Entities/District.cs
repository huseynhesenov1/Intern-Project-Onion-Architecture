using Project.Domain.Entities.Commons;

namespace Project.Domain.Entities;

public class District : BaseAuditableEntity
{
    public string Name { get; set; }
    public ICollection<Worker> Workers { get; set; }
    public ICollection<ProductDistrictPrice> ProductDistrictPrices { get; set; }
}
 
