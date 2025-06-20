using Project.Application.Abstractions.Repositories.Campaign;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories.Campaign;
public class CampaignWriteRepository : WriteRepository<Project.Domain.Entities.Campaign>, ICampaignWriteRepository
{
    private readonly AppDbContext _context;
    public CampaignWriteRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> SetActiveAsync(int id)
    {
        //var campaign = await _context.Campaigns.FindAsync(id);
        var campaign = await Table.FindAsync(id);
        if (campaign == null)
            return false;

        campaign.IsActive = !campaign.IsActive; 
        return true;
    }
}