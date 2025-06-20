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
        public async Task<ApiResponse<ResponseWorkerOutput>> Create([FromBody] CreateWorkerInput workerCreateDTO)
        {
            try
            {
                return await _workerService.CreateAsync(workerCreateDTO);

            }
            catch (Exception ex)
            {
                return ApiResponse<ResponseWorkerOutput>.Fail(ex.Message, "Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<bool>> Update(int id, [FromBody] UpdateWorkerInput workerUpdateDTO)
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
        public async Task<IActionResult> GetSearch([FromQuery] SearchWorkerInput workerSearchDTO)
        {
            var result = await _workerService.SearchProductsAsync(workerSearchDTO);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<CreateWorkerOutput>> GetById(int id)
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
