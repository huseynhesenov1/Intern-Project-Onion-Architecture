using Project.Application.Abstractions.Repositories;

namespace Project.Application.Abstractions.Repositories.Worker
{
    public interface IWorkerWriteRepository : IWriteRepository<Domain.Entities.Worker>
    {
    }
}
