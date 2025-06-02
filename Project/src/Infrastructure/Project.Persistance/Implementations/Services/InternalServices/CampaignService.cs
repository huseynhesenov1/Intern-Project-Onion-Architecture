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

        public async Task<ApiResponse<int>> CreateAsync(CampaignCreateDTO campaignCreateDTO)
        {
            var newStartDate = campaignCreateDTO.StartDate;
            var newEndDate = campaignCreateDTO.EndDate;

            var existingCampaigns = await _campaignReadRepository.GetAllAsync(false);

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

            await _campaignWriteRepository.CreateAsync(newCampaign);
            await _campaignWriteRepository.SaveChangeAsync();

            return ApiResponse<int>.Success(newCampaign.Id, "Kampaniya uğurla yaradıldı");
        }






        public async Task<ApiResponse<int>> UpdateAsync(int id, CampaignUpdateDTO campaignUpdateDTO)
        {
            var existingCampaign = await _campaignReadRepository.GetByIdAsync(id, true);
            if (existingCampaign == null || existingCampaign.IsDeleted)
            {
                return ApiResponse<int>.Fail("Yenilənəcək kampaniya tapılmadı.");
            }

            var newStartDate = campaignUpdateDTO.StartDate;
            var newEndDate = campaignUpdateDTO.EndDate;

            var otherCampaigns = await _campaignReadRepository.GetAllAsync(false);
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



        public async Task<ICollection<CampaignReadDTO>> GetAllAsync()
        {
            ICollection<Campaign> products = await _campaignReadRepository.GetAllAsync(false);
            List<CampaignReadDTO> productReadDTOs = products
        .Select(product => new CampaignReadDTO
        {
            Id = product.Id,
            IsActive = product.IsActive,
            Name = product.Name,
            Description = product.Description,
            StartDate = product.StartDate,
            EndDate = product.EndDate,
            DistrictId = product.DistrictId,
            DiscountPercent = product.DiscountPercent,
            UpdatedAt = product.UpdatedAt,
            CreatedAt = product.CreatedAt,

        }).ToList();
            return productReadDTOs;
        }


        public async Task<ApiResponse<CampaignReadDTO>> GetByIdAsync(int id)
        {
            try
            {
                var product = await _campaignReadRepository.GetByIdAsync(id, false);
                if (product == null || product.IsDeleted)
                {
                    return ApiResponse<CampaignReadDTO>.Fail("Campaign not found", "Invalid Campaign ID");
                }
                CampaignReadDTO productReadDTO = new CampaignReadDTO()
                {
                    Id = product.Id,
                    IsActive = product.IsActive,
                    Name = product.Name,
                    Description = product.Description,
                    StartDate = product.StartDate,
                    EndDate = product.EndDate,
                    DistrictId = product.DistrictId,
                    DiscountPercent = product.DiscountPercent,
                    UpdatedAt = product.UpdatedAt,
                    CreatedAt = product.CreatedAt,
                };

                return ApiResponse<CampaignReadDTO>.Success(productReadDTO, "Campaign retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CampaignReadDTO>.Fail(ex.Message, "Error retrieving Campaign");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var worker = await _campaignReadRepository.GetByIdAsync(id, true);
                if (worker == null || worker.IsDeleted)
                {
                    return ApiResponse<bool>.Fail("Campaign not found", "Invalid Campaign ID");
                }

                _campaignWriteRepository.SoftDelete(worker);
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
            var allCategories = await _campaignReadRepository.GetAllAsync(false);

            var filtered = allCategories
                .Skip((@params.PageNumber - 1) * @params.PageSize)
                .Take(@params.PageSize)
                .ToList();
            int totalCount = allCategories.Count;
            return new PagedResult<Campaign>(filtered, totalCount, @params.PageNumber, @params.PageSize);
        }

        public async Task<ApiResponse<bool>> EnableAsync(int id)
        {
            try
            {
                var worker = await _campaignReadRepository.GetByIdAsync(id, true);
                if (worker == null || worker.IsDeleted)
                {
                    return ApiResponse<bool>.Fail("Campaign not found", "Invalid Campaign ID");
                }
                if (worker.IsActive)
                {
                    return ApiResponse<bool>.Fail("Campaign Is Active", "Campaign Is Active");
                }

                worker.IsActive = true;
                _campaignWriteRepository.Update(worker);
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
                var worker = await _campaignReadRepository.GetByIdAsync(id, true);
                if (worker == null || worker.IsDeleted)
                {
                    return ApiResponse<bool>.Fail("Campaign not found", "Invalid Campaign ID");
                }
                if (!worker.IsActive)
                {
                    return ApiResponse<bool>.Fail("Campaign Is not Active", "Campaign Is not Active");
                }
                worker.IsActive = false;
                _campaignWriteRepository.Update(worker);
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
