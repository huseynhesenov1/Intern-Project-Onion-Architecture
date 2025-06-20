using Microsoft.EntityFrameworkCore;
using Project.Application.Abstractions.Repositories.Worker;
using Project.Application.Abstractions.Services.ExternalServices;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.WorkerDTOs;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Persistance.Implementations.Services.InternalServices
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerReadRepository _workerReadRepository;
        private readonly IWorkerWriteRepository _workerWriteRepository;

        private readonly IJwtService _jwtService;

        public WorkerService(IJwtService jwtService, IWorkerWriteRepository workerWriteRepository, IWorkerReadRepository workerReadRepository)
        {
            _jwtService = jwtService;
            _workerWriteRepository = workerWriteRepository;
            _workerReadRepository = workerReadRepository;
        }

        public async Task<ApiResponse<ResponseWorkerOutput>> CreateAsync(CreateWorkerInput workerCreateDTO)
        {
            try
            {
                var existingWorker = await _workerReadRepository.Table
                    .FirstOrDefaultAsync(w => w.FinCode == workerCreateDTO.FinCode && !w.IsDeleted);
                if (existingWorker != null)
                {
                    return ApiResponse<ResponseWorkerOutput>.Fail("Worker with this FinCode already exists", "Duplicate FinCode");
                }

                var worker = new Worker
                {
                    FinCode = workerCreateDTO.FinCode,
                    FullName = workerCreateDTO.FullName,
                    BirthDate = workerCreateDTO.BirthDate,
                    DistrictId = workerCreateDTO.DistrictId
                };
                await _workerWriteRepository.CreateAsync(worker);
                await _workerWriteRepository.SaveChangeAsync();
                var token = _jwtService.GenerateToken(worker);
                return ApiResponse<ResponseWorkerOutput>.Success(
                    new ResponseWorkerOutput
                    {
                        WorkerId = worker.Id,
                        WorkerToken = token
                    },
                    "Worker created successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<ResponseWorkerOutput>.Fail(ex.Message, "Error creating worker");
            }
        }
        public async Task<ApiResponse<bool>> UpdateAsync(int id, UpdateWorkerInput workerUpdateDTO)
        {
            try
            {
                var worker = await _workerReadRepository.GetByIdAsync(id, true);
                if (worker == null || worker.IsDeleted)
                {
                    return ApiResponse<bool>.Fail("Worker not found", "Invalid worker ID");
                }
                //burda Tbale muraciet etmeden yaz 
                var existingWorker = await _workerReadRepository.Table
                    .FirstOrDefaultAsync(w => w.FinCode == workerUpdateDTO.FinCode && w.Id != id && !w.IsDeleted);
                if (existingWorker != null)
                {
                    return ApiResponse<bool>.Fail("Worker with this FinCode already exists", "Duplicate FinCode");
                }
                worker.FinCode = workerUpdateDTO.FinCode;
                worker.FullName = workerUpdateDTO.FullName;
                worker.BirthDate = workerUpdateDTO.BirthDate;
                worker.DistrictId = workerUpdateDTO.DistrictId;
                worker.UpdatedAt = DateTime.UtcNow.AddHours(4);

                _workerWriteRepository.Update(worker);
                await _workerWriteRepository.SaveChangeAsync();

                return ApiResponse<bool>.Success(true, "Worker updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message, "Error updating worker");
            }
        }
        public async Task<ICollection<Worker>> GetAllAsync()
        {
            return await _workerReadRepository.GetAllAsync(false, false);
        }
        public async Task<PagedResult<Worker>> GetPaginatedAsync(PaginationParams @params)
        {
            var allWorkers = await _workerReadRepository.GetAllAsync(false, false);

            var filtered = allWorkers
                .Skip((@params.PageNumber - 1) * @params.PageSize)
                .Take(@params.PageSize)
                .ToList();
            int totalCount = allWorkers.Count;

            return new PagedResult<Worker>(filtered, totalCount, @params.PageNumber, @params.PageSize);
        }

        public async Task<ApiResponse<CreateWorkerOutput>> GetByIdAsync(int id)
        {
            try
            {
                var worker = await _workerReadRepository.GetByIdAsync(id, true);
                if (worker == null || worker.IsDeleted)
                {
                    return ApiResponse<CreateWorkerOutput>.Fail("Worker not found", "Invalid worker ID");
                }

                var workerDto = new CreateWorkerOutput
                {
                    WorkerId = worker.Id,
                    FinCode = worker.FinCode,
                    FullName = worker.FullName,
                    BirthDate = worker.BirthDate,
                    DistrictId = worker.DistrictId,
                };

                return ApiResponse<CreateWorkerOutput>.Success(workerDto, "Worker retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CreateWorkerOutput>.Fail(ex.Message, "Error retrieving worker");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var worker = await _workerReadRepository.GetByIdAsync(id, true);
                if (worker == null || worker.IsDeleted)
                {
                    return ApiResponse<bool>.Fail("Worker not found", "Invalid worker ID");
                }

                _workerWriteRepository.SoftDelete(worker);
                await _workerWriteRepository.SaveChangeAsync();

                return ApiResponse<bool>.Success(true, "Worker deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message, "Error deleting worker");
            }
        }


        public async Task<ICollection<CreateWorkerOutput>> SearchProductsAsync(SearchWorkerInput workerSearchDTO)
        {
            var query = await _workerReadRepository.GetAllAsync(false, false);

            query = query
    .Where(p =>
        (string.IsNullOrWhiteSpace(workerSearchDTO.FullName) ||
         p.FullName.Contains(workerSearchDTO.FullName, StringComparison.OrdinalIgnoreCase)) &&

        (string.IsNullOrWhiteSpace(workerSearchDTO.FinCode) ||
         p.FinCode.Contains(workerSearchDTO.FinCode, StringComparison.OrdinalIgnoreCase)) &&

        (workerSearchDTO.DistrictId == null ||
         p.DistrictId == workerSearchDTO.DistrictId) &&

        (workerSearchDTO.BirthDate == null ||
         p.BirthDate == workerSearchDTO.BirthDate)
    )
    .ToList();

            var workerDTOs = query.Select(p => new CreateWorkerOutput
            {
                WorkerId = p.Id,
                FullName = p.FullName,
                FinCode = p.FinCode,
                BirthDate = p.BirthDate,
                DistrictId = p.DistrictId,
            }).ToList();

            return workerDTOs;
        }
    }
}
