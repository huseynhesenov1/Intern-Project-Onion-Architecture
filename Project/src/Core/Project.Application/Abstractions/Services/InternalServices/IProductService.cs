using Project.Application.DTOs.ProductDTOs;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Services.InternalServices
{
    public interface IProductService
    {
        Task<int> CreateAsync(CreateProductInput productCreateDTO);
        Task<int> UpdateAsync(int Id, UpdateProductInput productUpdateDTO);
        Task<ICollection<CreateProductOutput>> GetAllAsync();
        //Task<PagedResult<Product>> GetPaginatedAsync(PaginationParams paginationParams);
        Task<PagedResult<CreateProductOutput>> GetPaginatedAsync(PaginationParams paginationParams);
        Task<bool> DeleteAsync(int id);
        Task<CreateProductOutput> GetByIdAsync(int id);
        Task<ICollection<CreateProductOutput>> SearchProductsAsync(string title);
    }
}
