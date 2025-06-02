using Project.Application.Abstractions.Repositories.Campaign;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories;

public class CampaignReadRepository : ReadRepository<Project.Domain.Entities.Campaign>, ICampaignReadRepository
{
    public CampaignReadRepository(AppDbContext context) : base(context)
    {
    }
}
