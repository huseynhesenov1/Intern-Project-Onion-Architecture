using Project.Application.DTOs.WorkerDTOs;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Services.InternalServices
{
    public interface IWorkerService
    {
        Task<ICollection<Worker>> GetAllAsync();
        Task<ApiResponse<ResponseWorkerOutput>> CreateAsync(CreateWorkerInput workerCreateDTO);
        Task<PagedResult<Worker>> GetPaginatedAsync(PaginationParams @params);
        Task<ApiResponse<bool>> UpdateAsync(int id, UpdateWorkerInput workerUpdateDTO);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<CreateWorkerOutput>> GetByIdAsync(int id);
        Task<ICollection<CreateWorkerOutput>> SearchProductsAsync(SearchWorkerInput workerSearchDTO);
    }
}
