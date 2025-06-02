using Project.Application.Abstractions.Repositories.District;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.District
{
    public class DistrictWriteRepository : WriteRepository<Domain.Entities.District>, IDistrictWriteRepository
    {
        public DistrictWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
