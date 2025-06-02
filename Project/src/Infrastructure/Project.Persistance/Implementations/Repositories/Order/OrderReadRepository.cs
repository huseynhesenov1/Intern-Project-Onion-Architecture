using Project.Application.Abstractions.Repositories.Order;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.Order
{
    public class OrderReadRepository : ReadRepository<Domain.Entities.Order>, IOrderReadRepository
    {
        public OrderReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
