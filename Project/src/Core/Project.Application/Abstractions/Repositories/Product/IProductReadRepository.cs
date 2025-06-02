using Project.Application.Abstractions.Repositories;

namespace Project.Application.Abstractions.Repositories.Product
{
    public interface IProductReadRepository : IReadRepository<Domain.Entities.Product>
    {
    }
}
