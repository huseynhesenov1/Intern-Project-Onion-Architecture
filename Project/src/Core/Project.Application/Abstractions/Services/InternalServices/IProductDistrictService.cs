using Project.Application.DTOs.ProductDistrictPriceDTOs;
using Project.Domain.Entities;

namespace Project.Application.Abstractions.Services.InternalServices
{
    public interface IProductDistrictService
    {
        Task<ICollection<ProductDistrictPrice>> GetAllAsync();
        Task<ProductDistrictPrice> CreateAsync(ProductDistrictPriceCreateDTO dto);
    }
}
