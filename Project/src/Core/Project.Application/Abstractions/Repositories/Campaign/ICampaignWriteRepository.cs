using Project.Application.Abstractions.Repositories;
using Project.Application.Models;

namespace Project.Application.Abstractions.Repositories.Campaign
{
    public interface ICampaignWriteRepository : IWriteRepository<Domain.Entities.Campaign>
    {
        Task<bool> SetActiveAsync(int id);
    }
}
