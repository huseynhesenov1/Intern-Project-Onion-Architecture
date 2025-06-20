using Project.Application.DTOs.ProductDTOs;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Services.InternalServices
{
    public interface IProductService
    {
        Task<int> CreateAsync(CreateProductInput productCreateDTO);
        Task<int> UpdateAsync(int Id, UpdateProductInput productUpdateDTO);
        Task<ICollection<CreateProductOutput>> GetAllAsync();
        Task<PagedResult<Product>> GetPaginatedAsync(PaginationParams @params);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<CreateProductOutput>> GetByIdAsync(int id);
        Task<ICollection<CreateProductOutput>> SearchProductsAsync(string title);
    }
}
