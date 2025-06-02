using Project.Application.Abstractions.Repositories;

namespace Project.Application.Abstractions.Repositories.Product
{
    public interface IProductWriteRepository : IWriteRepository<Domain.Entities.Product>
    {
    }
}
