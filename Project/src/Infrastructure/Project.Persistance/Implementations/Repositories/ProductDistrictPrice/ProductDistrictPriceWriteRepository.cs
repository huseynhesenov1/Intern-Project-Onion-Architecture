using Project.Application.Abstractions.Repositories.ProductDistrictPrice;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.ProductDistrictPrice
{
    public class ProductDistrictPriceWriteRepository : WriteRepository<Project.Domain.Entities.ProductDistrictPrice>, IProductDistrictPriceWriteRepository
    {
        public ProductDistrictPriceWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
