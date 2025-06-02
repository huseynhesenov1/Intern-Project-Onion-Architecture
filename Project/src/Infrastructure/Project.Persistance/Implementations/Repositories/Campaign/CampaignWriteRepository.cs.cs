using Project.Application.Abstractions.Repositories.Campaign;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.Campaign;
public class CampaignWriteRepository : WriteRepository<Project.Domain.Entities.Campaign>, ICampaignWriteRepository
{
    public CampaignWriteRepository(AppDbContext context) : base(context)
    {
    }
}