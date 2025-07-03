using Microsoft.AspNetCore.Mvc;
using Project.API.Models;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.WorkerDTOs;
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
            return Ok(ApiResponse<ResponseWorkerOutput>.Success(result, "Usta uğurla yaradıldı"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<ResponseWorkerOutput>.Fail("Yaratma zamanı xəta baş verdi", ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkerInput input)
    {
        try
        {
            await _workerService.UpdateAsync(id, input);
            return Ok(ApiResponse<bool>.Success(true, "Usta uğurla yeniləndi"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<bool>.Fail("Yeniləmə zamanı xəta baş verdi", ex.Message));
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllWorkers()
    {
        try
        {
            var result = await _workerService.GetAllAsync();
            return Ok(ApiResponse<ICollection<Worker>>.Success(result, "Bütün ustalar uğurla alındı"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ICollection<Worker>>.Fail("Xəta baş verdi", ex.Message));
        }
    }

    [HttpGet("Paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] PaginationParams paginationParams)
    {
        try
        {
            var result = await _workerService.GetPaginatedAsync(paginationParams);
            return Ok(ApiResponse<PagedResult<CreateWorkerOutput>>.Success(result, "Səhifələnmiş siyahı uğurla alındı"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<PagedResult<Worker>>.Fail("Paged nəticələr alına bilmədi", ex.Message));
        }
    }

    [HttpGet("Search")]
    public async Task<IActionResult> GetSearch([FromQuery] SearchWorkerInput input)
    {
        try
        {
            var result = await _workerService.SearchProductsAsync(input);
            return Ok(ApiResponse<ICollection<CreateWorkerOutput>>.Success(result, "Axtarış nəticələri alındı"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ICollection<CreateWorkerOutput>>.Fail("Axtarış zamanı xəta baş verdi", ex.Message));
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _workerService.GetByIdAsync(id);
            return Ok(ApiResponse<CreateWorkerOutput>.Success(result, "Usta uğurla tapıldı"));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse<CreateWorkerOutput>.Fail("Usta tapılmadı", ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _workerService.DeleteAsync(id);
            return Ok(ApiResponse<bool>.Success(true, "Usta uğurla silindi"));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse<bool>.Fail("Silinmə zamanı xəta baş verdi", ex.Message));
        }
    }
}



