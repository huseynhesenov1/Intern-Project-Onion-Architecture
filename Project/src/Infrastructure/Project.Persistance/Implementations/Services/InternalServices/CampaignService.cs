using Project.Application.Abstractions.Repositories.Campaign;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.Campaign;
using Project.Application.Exceptions;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Persistance.Implementations.Services.InternalServices;

public class CampaignService : ICampaignService
{
    private readonly ICampaignReadRepository _campaignReadRepository;
    private readonly ICampaignWriteRepository _campaignWriteRepository;

    public CampaignService(ICampaignReadRepository campaignReadRepository,
                           ICampaignWriteRepository campaignWriteRepository)
    {
        _campaignReadRepository = campaignReadRepository;
        _campaignWriteRepository = campaignWriteRepository;
    }

    public async Task<int> CreateAsync(CreateCampaignInput input)
    {
        var newStartDate = input.StartDate;
        var newEndDate = input.EndDate;


        var campaigns = await _campaignReadRepository.GetAllAsync(false, false);

        var conflict = campaigns.FirstOrDefault(c =>
                !c.IsDeleted &&
                ((newStartDate >= c.StartDate && newStartDate <= c.EndDate) ||
                 (newEndDate >= c.StartDate && newEndDate <= c.EndDate) ||
                 (newStartDate <= c.StartDate && newEndDate >= c.EndDate))
            );


        if (conflict != null)
            throw new CampaignConflictException(conflict.Name);

        Campaign entity = new()
        {
            Name = input.Name,
            Description = input.Description,
            StartDate = input.StartDate,
            EndDate = input.EndDate,
            DiscountPercent = input.DiscountPercent,
            DistrictId = input.DistrictId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        var created = await _campaignWriteRepository.CreateAsync(entity);
        if (created == null)
            throw new Exception("Kampaniya yaradılmadı.");

        await _campaignWriteRepository.SaveChangeAsync();
        return entity.Id;
    }

    public async Task<int> UpdateAsync(int id, UpdateCampaignInput input)
    {
        var existing = await _campaignReadRepository.GetByIdAsync(id, true);
        if (existing == null || existing.IsDeleted)
            throw new CampaignNotFoundException(id);

        var others = await _campaignReadRepository.GetAllAsync(false, false);
        var conflict = others.FirstOrDefault(c =>
            c.Id != id && !c.IsDeleted &&
            ((input.StartDate >= c.StartDate && input.StartDate <= c.EndDate) ||
             (input.EndDate >= c.StartDate && input.EndDate <= c.EndDate) ||
             (input.StartDate <= c.StartDate && input.EndDate >= c.EndDate))
        );

        if (conflict != null)
            throw new CampaignConflictException(conflict.Name);

        existing.Name = input.Name;
        existing.Description = input.Description;
        existing.StartDate = input.StartDate;
        existing.EndDate = input.EndDate;
        existing.DiscountPercent = input.DiscountPercent;
        existing.DistrictId = input.DistrictId;
        existing.UpdatedAt = DateTime.UtcNow;

        _campaignWriteRepository.Update(existing);
        await _campaignWriteRepository.SaveChangeAsync();

        return existing.Id;
    }

    public async Task<ICollection<CampaignOutput>> GetAllAsync()
    {
        var campaigns = await _campaignReadRepository.GetAllAsync(false, false);
        return campaigns.Select(c => new CampaignOutput
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            DiscountPercent = c.DiscountPercent,
            DistrictId = c.DistrictId,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            IsActive = c.IsActive
        }).ToList();
    }

    public async Task<CampaignOutput> GetByIdAsync(int id)
    {
        var c = await _campaignReadRepository.GetByIdAsync(id, false);
        if (c == null || c.IsDeleted)
            throw new CampaignNotFoundException(id);

        return new CampaignOutput
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            DiscountPercent = c.DiscountPercent,
            DistrictId = c.DistrictId,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            IsActive = c.IsActive
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var campaign = await _campaignReadRepository.GetByIdAsync(id, true);
        if (campaign == null || campaign.IsDeleted)
            throw new CampaignNotFoundException(id);

        _campaignWriteRepository.SoftDelete(campaign);
        await _campaignWriteRepository.SaveChangeAsync();
        return true;
    }

    public async Task<bool> EnableAsync(int id)
    {
        var campaign = await _campaignReadRepository.GetByIdAsync(id, true);
        if (campaign == null)
            throw new CampaignNotFoundException(id);

        if (campaign.IsActive)
            throw new InvalidOperationException("Bu kampaniya artıq aktivdir.");

        await _campaignWriteRepository.SetActiveAsync(id);
        await _campaignWriteRepository.SaveChangeAsync();
        return true;
    }

    public async Task<bool> DisableAsync(int id)
    {
        var campaign = await _campaignReadRepository.GetByIdAsync(id, true);
        if (campaign == null)
            throw new CampaignNotFoundException(id);

        if (!campaign.IsActive)
            throw new InvalidOperationException("Bu kampaniya artıq qeyri-aktivdir.");

        await _campaignWriteRepository.SetActiveAsync(id);
        await _campaignWriteRepository.SaveChangeAsync();
        return true;
    }

    public async Task<PagedResult<Campaign>> GetPaginatedAsync(PaginationParams @params)
    {
        var all = await _campaignReadRepository.GetAllAsync(false, false);
        var filtered = all.Skip((@params.PageNumber - 1) * @params.PageSize)
                          .Take(@params.PageSize)
                          .ToList();

        return new PagedResult<Campaign>(filtered, all.Count, @params.PageNumber, @params.PageSize);
    }
}


