using Project.Application.DTOs.Campaign;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Services.InternalServices;

public interface ICampaignService
{
    Task<int> CreateAsync(CreateCampaignInput campaignCreateDTO);
    Task<int> UpdateAsync(int id, UpdateCampaignInput campaignUpdateDTO);
    Task<ICollection<CampaignOutput>> GetAllAsync();
    Task<CampaignOutput> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<bool> EnableAsync(int id);
    Task<bool> DisableAsync(int id);
    Task<PagedResult<CampaignOutput>> GetPaginatedAsync(PaginationParams paginationParams);
    

}
