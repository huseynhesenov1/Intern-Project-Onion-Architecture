using Project.Application.Abstractions.Repositories;

namespace Project.Application.Abstractions.Repositories.Campaign
{
    public interface ICampaignWriteRepository : IWriteRepository<Domain.Entities.Campaign>
    {
    }
}
