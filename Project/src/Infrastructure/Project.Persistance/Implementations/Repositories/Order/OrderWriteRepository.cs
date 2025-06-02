using Project.Application.Abstractions.Repositories.Order;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.Order
{
    public class OrderWriteRepository : WriteRepository<Domain.Entities.Order>, IOrderWriteRepository
    {
        public OrderWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
