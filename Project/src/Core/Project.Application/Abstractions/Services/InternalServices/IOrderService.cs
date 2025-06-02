using Project.Application.DTOs.OrderDTOs;
using Project.Application.Models;
using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Services.InternalServices
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderCreateResponseDTO>> CreateAsync(OrderCreateDTO dto);
        Task<ICollection<OrderReadDTO>> GetAllAsync();
        Task<PagedResult<OrderReadDTO>> GetPaginatedAsync(PaginationParams @params);
    }
}