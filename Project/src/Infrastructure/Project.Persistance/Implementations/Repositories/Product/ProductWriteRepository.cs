using Project.Application.Abstractions.Repositories.Product;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.Product
{
    public class ProductWriteRepository : WriteRepository<Domain.Entities.Product>, IProductWriteRepository
    {
        public ProductWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
