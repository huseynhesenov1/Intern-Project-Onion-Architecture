using Project.Application.Abstractions.Repositories.Campaign;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.Campaign;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Persistance.Implementations.Services.InternalServices
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignReadRepository _campaignReadRepository;
        private readonly ICampaignWriteRepository _campaignWriteRepository;

        public CampaignService(ICampaignReadRepository campaignReadRepository,
            ICampaignWriteRepository campaignWriteRepository
            )
        {
            _campaignReadRepository = campaignReadRepository;
            _campaignWriteRepository = campaignWriteRepository;

        }

        public async Task<ApiResponse<int>> CreateAsync(CreateCampaignInput campaignCreateDTO)
        {
            var newStartDate = campaignCreateDTO.StartDate;
            var newEndDate = campaignCreateDTO.EndDate;

            var existingCampaigns = await _campaignReadRepository.GetAllAsync(false, false);

            var conflictingCampaign = existingCampaigns.FirstOrDefault(c =>
                !c.IsDeleted &&
                ((newStartDate >= c.StartDate && newStartDate <= c.EndDate) ||
                 (newEndDate >= c.StartDate && newEndDate <= c.EndDate) ||
                 (newStartDate <= c.StartDate && newEndDate >= c.EndDate))
            );

            if (conflictingCampaign != null)
            {
                return ApiResponse<int>.Fail(
                    $"Yeni kampaniya yaradılmadı. '{conflictingCampaign.Name}' adlı kampaniya ilə tarixlər üst-üstə düşür.",
                    "Tarix konflikti");
            }

            Campaign newCampaign = new Campaign
            {
                Name = campaignCreateDTO.Name,
                Description = campaignCreateDTO.Description,
                StartDate = campaignCreateDTO.StartDate,
                EndDate = campaignCreateDTO.EndDate,
                DiscountPercent = campaignCreateDTO.DiscountPercent,
                DistrictId = campaignCreateDTO.DistrictId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var res = await _campaignWriteRepository.CreateAsync(newCampaign);
            if (res == null)
            {
                return ApiResponse<int>.Fail("Kampaniya yaradılmadı", "Kampaniya yaradılmadı");
            }
            await _campaignWriteRepository.SaveChangeAsync();

            return ApiResponse<int>.Success(newCampaign.Id, "Kampaniya uğurla yaradıldı");
        }






        public async Task<ApiResponse<int>> UpdateAsync(int id, UpdateCampaignInput campaignUpdateDTO)
        {
            var existingCampaign = await _campaignReadRepository.GetByIdAsync(id, true);
            if (existingCampaign == null || existingCampaign.IsDeleted)
            {
                return ApiResponse<int>.Fail("Yenilənəcək kampaniya tapılmadı.");
            }

            var newStartDate = campaignUpdateDTO.StartDate;
            var newEndDate = campaignUpdateDTO.EndDate;

            var otherCampaigns = await _campaignReadRepository.GetAllAsync(false, false);
            var conflictingCampaign = otherCampaigns.FirstOrDefault(c =>
                c.Id != id && !c.IsDeleted &&
                ((newStartDate >= c.StartDate && newStartDate <= c.EndDate) ||
                 (newEndDate >= c.StartDate && newEndDate <= c.EndDate) ||
                 (newStartDate <= c.StartDate && newEndDate >= c.EndDate))
            );

            if (conflictingCampaign != null)
            {
                return ApiResponse<int>.Fail(
                    "Yenilənmə baş tutmadı.",
                    $"'{conflictingCampaign.Name}' adlı kampaniya ilə tarixlər üst-üstə düşür.");
            }

            existingCampaign.Name = campaignUpdateDTO.Name;
            existingCampaign.Description = campaignUpdateDTO.Description;
            existingCampaign.StartDate = campaignUpdateDTO.StartDate;
            existingCampaign.EndDate = campaignUpdateDTO.EndDate;
            existingCampaign.DiscountPercent = campaignUpdateDTO.DiscountPercent;
            existingCampaign.DistrictId = campaignUpdateDTO.DistrictId;
            existingCampaign.UpdatedAt = DateTime.UtcNow;

            _campaignWriteRepository.Update(existingCampaign);
            await _campaignWriteRepository.SaveChangeAsync();

            return ApiResponse<int>.Success(existingCampaign.Id, "Kampaniya uğurla yeniləndi.");
        }



        public async Task<ICollection<CampaignOutput>> GetAllAsync()
        {
            ICollection<Campaign> campaigns = await _campaignReadRepository.GetAllAsync(false, false);
            List<CampaignOutput> campaignReadDTOs = campaigns
        .Select(campaign => new CampaignOutput
        {
            Id = campaign.Id,
            IsActive = campaign.IsActive,
            Name = campaign.Name,
            Description = campaign.Description,
            StartDate = campaign.StartDate,
            EndDate = campaign.EndDate,
            DistrictId = campaign.DistrictId,
            DiscountPercent = campaign.DiscountPercent,
            UpdatedAt = campaign.UpdatedAt,
            CreatedAt = campaign.CreatedAt,

        }).ToList();
            return campaignReadDTOs;
        }


        public async Task<ApiResponse<CampaignOutput>> GetByIdAsync(int id)
        {
            try
            {
                var campaign = await _campaignReadRepository.GetByIdAsync(id, false);
                if (campaign == null || campaign.IsDeleted)
                {
                    return ApiResponse<CampaignOutput>.Fail("Campaign not found", "Invalid Campaign ID");
                }
                CampaignOutput campaignReadDTO = new CampaignOutput()
                {
                    Id = campaign.Id,
                    IsActive = campaign.IsActive,
                    Name = campaign.Name,
                    Description = campaign.Description,
                    StartDate = campaign.StartDate,
                    EndDate = campaign.EndDate,
                    DistrictId = campaign.DistrictId,
                    DiscountPercent = campaign.DiscountPercent,
                    UpdatedAt = campaign.UpdatedAt,
                    CreatedAt = campaign.CreatedAt,
                };

                return ApiResponse<CampaignOutput>.Success(campaignReadDTO, "Campaign retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CampaignOutput>.Fail(ex.Message, "Error retrieving Campaign");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var campaign = await _campaignReadRepository.GetByIdAsync(id, true);
                if (campaign == null || campaign.IsDeleted)
                {
                    return ApiResponse<bool>.Fail("Campaign not found", "Invalid Campaign ID");
                }

                _campaignWriteRepository.SoftDelete(campaign);
                await _campaignWriteRepository.SaveChangeAsync();

                return ApiResponse<bool>.Success(true, "Campaign deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message, "Error deleting Campaign");
            }
        }

        public async Task<PagedResult<Campaign>> GetPaginatedAsync(PaginationParams @params)
        {
            var allCampagions = await _campaignReadRepository.GetAllAsync(false, false);

            var filtered = allCampagions
                .Skip((@params.PageNumber - 1) * @params.PageSize)
                .Take(@params.PageSize)
                .ToList();
            int totalCount = allCampagions.Count;
            return new PagedResult<Campaign>(filtered, totalCount, @params.PageNumber, @params.PageSize);
        }

        public async Task<ApiResponse<bool>> EnableAsync(int id)
        {
            try
            {
                var campaign = await _campaignReadRepository.GetByIdAsync(id, true);
                if (campaign == null)
                {
                    return ApiResponse<bool>.Fail("Campaign not found", "Invalid Campaign ID");
                }
                if (campaign.IsActive)
                {
                    return ApiResponse<bool>.Fail("Bu kampaniya artıq aktivdir.", "This campaign is already active.");
                }

                await _campaignWriteRepository.SetActiveAsync(id);
                await _campaignWriteRepository.SaveChangeAsync();

                return ApiResponse<bool>.Success(true, "Campaign enabled successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message, "Error enabled Campaign");
            }
        }
        public async Task<ApiResponse<bool>> DisableAsync(int id)
        {
            try
            {
                var campaign = await _campaignReadRepository.GetByIdAsync(id, true);
                if (campaign == null)
                {
                    return ApiResponse<bool>.Fail("Campaign not found", "Invalid Campaign ID");
                }
                if (!campaign.IsActive)
                {
                    return ApiResponse<bool>.Fail("Bu kampaniya artıq qeyri-aktivdir.", "This campaign is already inactive.");
                }
                await _campaignWriteRepository.SetActiveAsync(id);
                await _campaignWriteRepository.SaveChangeAsync();

                return ApiResponse<bool>.Success(true, "Campaign disabled successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message, "Error disabled Campaign");
            }
        }
    }
}
