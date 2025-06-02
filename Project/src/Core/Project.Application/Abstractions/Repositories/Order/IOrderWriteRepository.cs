using Project.Application.Abstractions.Repositories;

namespace Project.Application.Abstractions.Repositories.Order
{
    public interface IOrderWriteRepository : IWriteRepository<Domain.Entities.Order>
    {
    }
}
