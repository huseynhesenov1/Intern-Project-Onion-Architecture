using Project.Application.Abstractions.Repositories;

namespace Project.Application.Abstractions.Repositories.Worker
{
    public interface IWorkerReadRepository : IReadRepository<Domain.Entities.Worker>
    {
    }
}
