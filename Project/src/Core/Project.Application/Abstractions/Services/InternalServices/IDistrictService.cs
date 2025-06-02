using Project.Application.DTOs.DistrictDTOs;
using Project.Domain.Entities;

namespace Project.Application.Abstractions.Services.InternalServices
{
    public interface IDistrictService
    {
        Task<ICollection<DistrictReadDTO>> GetAllAsync();
        Task<District> GetByIdAsync(int id);
        Task<District> CreateAsync(DistrictCreateDTO studentCreateDTO);
        Task<District> UpdateAsync(int id, DistrictUpdateDTO studentUpdateDTO);
        Task<District> SoftDeleteAsync(int id);
    }
}
