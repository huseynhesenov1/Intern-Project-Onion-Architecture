using Project.Application.DTOs.ProductDTOs;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Services.InternalServices
{
    public interface IProductService
    {
        Task<int> CreateAsync(ProductCreateDTO productCreateDTO);
        Task<int> UpdateAsync(int Id, ProductUpdateDTO productUpdateDTO);
        Task<ICollection<ProductReadDTO>> GetAllAsync();
        Task<PagedResult<Product>> GetPaginatedAsync(PaginationParams @params);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<ProductReadDTO>> GetByIdAsync(int id);
        Task<ICollection<ProductReadDTO>> SearchProductsAsync(string title);
    }
}
