using Project.Application.Abstractions.Repositories;

namespace Project.Application.Abstractions.Repositories.Order
{
    public interface IOrderReadRepository : IReadRepository<Domain.Entities.Order>
    {
    }
}
