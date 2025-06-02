using Project.Application.Abstractions.Repositories.Worker;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.Worker
{
    public class WorkerWriteRepository : WriteRepository<Domain.Entities.Worker>, IWorkerWriteRepository
    {
        public WorkerWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
