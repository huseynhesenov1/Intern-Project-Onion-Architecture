using Project.Application.DTOs.WorkerDTOs;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Services.InternalServices
{
    public interface IWorkerService
    {
        Task<ICollection<Worker>> GetAllAsync();
        Task<ApiResponse<WorkerCreateResponseDTO>> CreateAsync(WorkerCreateDTO workerCreateDTO);
        Task<PagedResult<Worker>> GetPaginatedAsync(PaginationParams @params);
        Task<ApiResponse<bool>> UpdateAsync(int id, WorkerUpdateDTO workerUpdateDTO);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<WorkerDTO>> GetByIdAsync(int id);
        Task<ICollection<WorkerDTO>> SearchProductsAsync(WorkerSearchDTO workerSearchDTO);
    }
}
