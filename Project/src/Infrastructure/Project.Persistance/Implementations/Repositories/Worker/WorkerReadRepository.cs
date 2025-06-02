using Project.Application.Abstractions.Repositories.Worker;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.Worker
{
    public class WorkerReadRepository : ReadRepository<Domain.Entities.Worker>, IWorkerReadRepository
    {
        public WorkerReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
