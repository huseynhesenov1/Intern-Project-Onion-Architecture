using Project.Application.DTOs.OrderDTOs;
using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Services.InternalServices
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateAsync(CreateOrderInput dto);
        Task<ICollection<CreateOrderOutput>> GetAllAsync();
        Task<PagedResult<CreateOrderOutput>> GetPaginatedAsync(PaginationParams paginationParams);
    }
}