using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.Campaign;
using Project.Application.Models;
using Project.Domain.Entities.Commons;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }
        [HttpGet]
        public async Task<ICollection<CampaignReadDTO>> GetAll()
        {
            return await _campaignService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ApiResponse<CampaignReadDTO>> GetById(int id)
        {
            return await _campaignService.GetByIdAsync(id);
        }
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CampaignCreateDTO campaignCreateDTO)
        {
            var result = await _campaignService.CreateAsync(campaignCreateDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    error = result.Message,
                    detail = result.ErrorDetail
                });
            }

            return Ok(new
            {
                id = result.Data,
                message = result.Message
            });
        }


        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] CampaignUpdateDTO productUpdateDTO)
        {
            var response = await _campaignService.UpdateAsync(Id, productUpdateDTO);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }



        [HttpGet("Paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] PaginationParams @params)
        {
            var result = await _campaignService.GetPaginatedAsync(@params);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<bool>> Delete(int id)
        {
            return await _campaignService.DeleteAsync(id);
        }
        [HttpPut("{id}Enable")]
        public async Task<ApiResponse<bool>> Enable(int id)
        {
            return await _campaignService.EnableAsync(id);
        }
        [HttpPut("{id}Disable")]
        public async Task<ApiResponse<bool>> Disable(int id)
        {
            return await _campaignService.DisableAsync(id);
        }

    }
}
