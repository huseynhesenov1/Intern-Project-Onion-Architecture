using Project.Application.DTOs.WorkerDTOs;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Services.InternalServices
{
    public interface IWorkerService
    {
        Task<ICollection<Worker>> GetAllAsync();
        Task<ResponseWorkerOutput> CreateAsync(CreateWorkerInput workerCreateDTO);
        Task<PagedResult<Worker>> GetPaginatedAsync(PaginationParams @params);
        Task<bool> UpdateAsync(int id, UpdateWorkerInput workerUpdateDTO);
        Task<bool> DeleteAsync(int id);
        Task<CreateWorkerOutput> GetByIdAsync(int id);
        Task<ICollection<CreateWorkerOutput>> SearchProductsAsync(SearchWorkerInput workerSearchDTO);
       
    }
}
 