using Project.Application.DTOs.DistrictDTOs;
using Project.Domain.Entities;

namespace Project.Application.Abstractions.Services.InternalServices
{
    public interface IDistrictService
    {
        Task<ICollection<CreateDistrictOutput>> GetAllAsync();
        Task<District> GetByIdAsync(int id);
        Task<District> CreateAsync(CreateDistrictInput studentCreateDTO);
        Task<District> UpdateAsync(int id, UpdateDistrictInput studentUpdateDTO);
        Task<District> SoftDeleteAsync(int id);
    }
}
