using Project.Application.Abstractions.Repositories.District;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.DistrictDTOs;
using Project.Domain.Entities;
using Project.Application.Abstractions.UnitOfWork;

namespace Project.Persistance.Implementations.Services.InternalServices
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictReadRepository _districtReadRepository;
        private readonly IDistrictWriteRepository _districtWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DistrictService(IDistrictWriteRepository districtWriteRepository, IDistrictReadRepository districtReadRepository, IUnitOfWork unitOfWork)
        {
            _districtWriteRepository = districtWriteRepository;
            _districtReadRepository = districtReadRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<District> CreateAsync(CreateDistrictInput districtCreateDTO)
        {
            District district = new District()
            {
                Name = districtCreateDTO.Name,
            };
            await _districtWriteRepository.CreateAsync(district);
            await _unitOfWork.SaveChangesAsync();
            return district;
        }


        public async Task<District> UpdateAsync(int Id, UpdateDistrictInput districtUpdateDTO)
        {
            District district = await _districtReadRepository.GetByIdAsync(Id);
            if (district == null)
            {
                throw new Exception("Invalid ID");
            }
            District newDistrict = new District()
            {
                Name = districtUpdateDTO.Name,
            };
            newDistrict.Id = Id;
            newDistrict.CreatedAt = district.CreatedAt;
            newDistrict.UpdatedAt = DateTime.UtcNow.AddHours(4);
            _districtWriteRepository.Update(newDistrict);
            await _unitOfWork.SaveChangesAsync();
            return newDistrict;
        }



        public async Task<ICollection<CreateDistrictOutput>> GetAllAsync()
        {
            ICollection<District> districts = await _districtReadRepository.GetAllAsync(false);
            List<CreateDistrictOutput> districtReadDTOs = districts
                .Select(d => new CreateDistrictOutput
                {
                    Id = d.Id,
                    Name = d.Name,
                })
                .ToList();
            return districtReadDTOs;
        }

        public async Task<District> GetByIdAsync(int id)
        {
            District district = await _districtReadRepository.GetByIdAsync(id);
            if (district is null)
            {
                throw new Exception("Bu Id-e uygun deyer tapilmadi");
            }
            return district;
        }



        public async Task<District> SoftDeleteAsync(int id)
        {
            District district = await _districtReadRepository.GetByIdAsync(id);
            if (district is null)
            {
                throw new Exception("Bu Id-e uygun deyer tapilmadi");
            }
            var res = _districtWriteRepository.SoftDelete(district);
            await _unitOfWork.SaveChangesAsync();
            return res;
        }
    }
}
