using Project.Application.Abstractions.Repositories.Product;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.Product
{
    public class ProductReadRepository : ReadRepository<Domain.Entities.Product>, IProductReadRepository
    {
        public ProductReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
