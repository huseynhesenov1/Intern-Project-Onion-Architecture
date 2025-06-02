using Project.Application.Abstractions.Repositories.District;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.District
{
    public class DistrictReadRepository : ReadRepository<Project.Domain.Entities.District>, IDistrictReadRepository
    {
        public DistrictReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
