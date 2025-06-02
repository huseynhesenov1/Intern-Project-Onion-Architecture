using Project.Application.Abstractions.Repositories.ProductDistrictPrice;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.ProductDistrictPrice
{
    public class ProductDistrictPriceReadRepository : ReadRepository<Domain.Entities.ProductDistrictPrice>, IProductDistrictPriceReadRepository
    {
        public ProductDistrictPriceReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
