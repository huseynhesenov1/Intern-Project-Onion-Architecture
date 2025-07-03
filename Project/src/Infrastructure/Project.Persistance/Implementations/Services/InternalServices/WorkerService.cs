using Project.Application.Abstractions.Repositories.Worker;
using Project.Application.Abstractions.Services.ExternalServices;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.Abstractions.UnitOfWork;
using Project.Application.DTOs.WorkerDTOs;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

public class WorkerService : IWorkerService
{
    private readonly IWorkerReadRepository _workerReadRepository;
    private readonly IWorkerWriteRepository _workerWriteRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;

    public WorkerService(IJwtService jwtService,
                         IWorkerWriteRepository workerWriteRepository,
                         IWorkerReadRepository workerReadRepository,
                         IUnitOfWork unitOfWork)
    {
        _jwtService = jwtService;
        _workerWriteRepository = workerWriteRepository;
        _workerReadRepository = workerReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseWorkerOutput> CreateAsync(CreateWorkerInput input)
    {
        var workers = await _workerReadRepository.GetAllAsync(false);
        var existingWorker = workers.FirstOrDefault(w => w.FinCode == input.FinCode && !w.IsDeleted);

        if (existingWorker != null)
            throw new Exception("Worker with this FinCode already exists");

        var worker = new Worker
        {
            FinCode = input.FinCode,
            FullName = input.FullName,
            BirthDate = input.BirthDate,
            DistrictId = input.DistrictId
        };

        await _workerWriteRepository.CreateAsync(worker);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtService.GenerateToken(worker);

        return new ResponseWorkerOutput
        {
            WorkerId = worker.Id,
            WorkerToken = token
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateWorkerInput input)
    {
        var worker = await _workerReadRepository.GetByIdAsync(id);
        if (worker == null || worker.IsDeleted)
            throw new Exception("Worker not found");
        var workers = await _workerReadRepository.GetAllAsync(false);
        var existingWorker = workers.FirstOrDefault(w => w.FinCode == input.FinCode && !w.IsDeleted);
        if (existingWorker != null)
            throw new Exception("Worker with this FinCode already exists");

        worker.FinCode = input.FinCode;
        worker.FullName = input.FullName;
        worker.BirthDate = input.BirthDate;
        worker.DistrictId = input.DistrictId;
        worker.UpdatedAt = DateTime.UtcNow.AddHours(4);

        _workerWriteRepository.Update(worker);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<ICollection<Worker>> GetAllAsync()
    {
        return await _workerReadRepository.GetAllAsync(false);
    }

    public async Task<PagedResult<Worker>> GetPaginatedAsync(PaginationParams @params)
    {
        var allWorkers = await _workerReadRepository.GetAllAsync( false);
        var filtered = allWorkers
            .Skip((@params.PageNumber - 1) * @params.PageSize)
            .Take(@params.PageSize)
            .ToList();

        return new PagedResult<Worker>(filtered, allWorkers.Count, @params.PageNumber, @params.PageSize);
    }

    public async Task<CreateWorkerOutput> GetByIdAsync(int id)
    {
        var worker = await _workerReadRepository.GetByIdAsync(id);
        if (worker == null || worker.IsDeleted)
            throw new Exception("Worker not found");

        return new CreateWorkerOutput
        {
            WorkerId = worker.Id,
            FinCode = worker.FinCode,
            FullName = worker.FullName,
            BirthDate = worker.BirthDate,
            DistrictId = worker.DistrictId
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var worker = await _workerReadRepository.GetByIdAsync(id);
        if (worker == null || worker.IsDeleted)
            throw new Exception("Worker not found");

        _workerWriteRepository.SoftDelete(worker);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<ICollection<CreateWorkerOutput>> SearchProductsAsync(SearchWorkerInput input)
    {
        var query = await _workerReadRepository.GetAllAsync(false);

        query = query
            .Where(p =>
                (string.IsNullOrWhiteSpace(input.FullName) || p.FullName.Contains(input.FullName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(input.FinCode) || p.FinCode.Contains(input.FinCode, StringComparison.OrdinalIgnoreCase)) &&
                (input.DistrictId == null || p.DistrictId == input.DistrictId) &&
                (input.BirthDate == null || p.BirthDate == input.BirthDate))
            .ToList();

        return query.Select(p => new CreateWorkerOutput
        {
            WorkerId = p.Id,
            FullName = p.FullName,
            FinCode = p.FinCode,
            BirthDate = p.BirthDate,
            DistrictId = p.DistrictId
        }).ToList();
    }
}



