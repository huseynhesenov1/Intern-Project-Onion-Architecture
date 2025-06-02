using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.WorkerDTOs;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }



        [HttpPost]
        public async Task<ApiResponse<WorkerCreateResponseDTO>> Create([FromBody] WorkerCreateDTO workerCreateDTO)
        {
            try
            {
                return await _workerService.CreateAsync(workerCreateDTO);

            }
            catch (Exception ex)
            {
                return ApiResponse<WorkerCreateResponseDTO>.Fail(ex.Message, "Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<bool>> Update(int id, [FromBody] WorkerUpdateDTO workerUpdateDTO)
        {
            return await _workerService.UpdateAsync(id, workerUpdateDTO);
        }

        [HttpGet("all")]
        public async Task<ICollection<Worker>> GetAllWorkers()
        {
            return await _workerService.GetAllAsync();
        }

        [HttpGet("Paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] PaginationParams @params)
        {
            var result = await _workerService.GetPaginatedAsync(@params);
            return Ok(result);
        }
        [HttpGet("Search")]
        public async Task<IActionResult> GetSearch([FromQuery] WorkerSearchDTO workerSearchDTO)
        {
            var result = await _workerService.SearchProductsAsync(workerSearchDTO);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<WorkerDTO>> GetById(int id)
        {
            return await _workerService.GetByIdAsync(id);
        }



        [HttpDelete("{id}")]
        public async Task<ApiResponse<bool>> Delete(int id)
        {
            return await _workerService.DeleteAsync(id);
        }


    }
}
