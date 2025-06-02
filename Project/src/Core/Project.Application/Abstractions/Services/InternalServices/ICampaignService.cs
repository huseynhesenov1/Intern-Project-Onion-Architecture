using Project.Application.DTOs.Campaign;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Services.InternalServices;

public interface ICampaignService
{
    Task<ApiResponse<int>> CreateAsync(CampaignCreateDTO campaignCreateDTO);
    Task<ApiResponse<int>> UpdateAsync(int id, CampaignUpdateDTO campaignUpdateDTO);
    Task<ICollection<CampaignReadDTO>> GetAllAsync();
    Task<PagedResult<Campaign>> GetPaginatedAsync(PaginationParams @params);
    Task<ApiResponse<bool>> DeleteAsync(int id);
    Task<ApiResponse<bool>> EnableAsync(int id);
    Task<ApiResponse<bool>> DisableAsync(int id);
    Task<ApiResponse<CampaignReadDTO>> GetByIdAsync(int id);
}
