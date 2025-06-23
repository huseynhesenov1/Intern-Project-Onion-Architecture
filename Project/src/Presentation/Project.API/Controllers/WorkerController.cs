using Microsoft.AspNetCore.Mvc;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.WorkerDTOs;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

[ApiController]
[Route("api/[controller]")]
public class WorkerController : ControllerBase
{
    private readonly IWorkerService _workerService;

    public WorkerController(IWorkerService workerService)
    {
        _workerService = workerService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkerInput input)
    {
        try
        {
            var result = await _workerService.CreateAsync(input);
            return Ok(ApiResponse<ResponseWorkerOutput>.Success(result, "Worker created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<ResponseWorkerOutput>.Fail(ex.Message, "Creation failed"));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkerInput input)
    {
        try
        {
            await _workerService.UpdateAsync(id, input);
            return Ok(ApiResponse<bool>.Success(true, "Updated successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<bool>.Fail(ex.Message, "Update failed"));
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllWorkers()
    {
        var result = await _workerService.GetAllAsync();
        return Ok(ApiResponse<ICollection<Worker>>.Success(result, "List retrieved"));
    }

    [HttpGet("Paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] PaginationParams @params)
    {
        var result = await _workerService.GetPaginatedAsync(@params);
        return Ok(ApiResponse<PagedResult<Worker>>.Success(result, "Paginated list"));
    }

    [HttpGet("Search")]
    public async Task<IActionResult> GetSearch([FromQuery] SearchWorkerInput input)
    {
        var result = await _workerService.SearchProductsAsync(input);
        return Ok(ApiResponse<ICollection<CreateWorkerOutput>>.Success(result, "Search result"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _workerService.GetByIdAsync(id);
            return Ok(ApiResponse<CreateWorkerOutput>.Success(result, "Retrieved successfully"));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse<CreateWorkerOutput>.Fail(ex.Message, "Worker not found"));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _workerService.DeleteAsync(id);
            return Ok(ApiResponse<bool>.Success(true, "Deleted successfully"));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse<bool>.Fail(ex.Message, "Delete failed"));
        }
    }
}

