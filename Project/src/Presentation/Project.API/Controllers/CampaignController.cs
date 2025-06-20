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
        public async Task<ICollection<CampaignOutput>> GetAll()
        {
            return await _campaignService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ApiResponse<CampaignOutput>> GetById(int id)
        {
            return await _campaignService.GetByIdAsync(id);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCampaignInput input)
        {
            try
            {
                var result = await _campaignService.CreateAsync(input);

                if (!result.IsSuccess)
                    return BadRequest(result); 

                return Ok(result); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Fail("Xəta baş verdi", ex.Message));
            }
        }


        


        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] UpdateCampaignInput productUpdateDTO)
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _campaignService.DeleteAsync(id);

                if (!result.IsSuccess)
                    return NotFound(result);

                return Ok(result); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Fail( "Server xətası baş verdi", ex.Message));
            }
        }
        [HttpPut("{id}/Enable")]
        public async Task<IActionResult> Enable(int id)
        {
            try
            {
                var result = await _campaignService.EnableAsync(id);

                if (!result.IsSuccess)
                    return BadRequest(result);  

                return Ok(result);  
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<bool>.Fail( "Server xətası baş verdi", ex.Message));
            }
            
        }
        [HttpPut("{id}/Disable")]
        public async Task<IActionResult> Disable(int id)
        {
            try
            {
                var result = await _campaignService.DisableAsync(id);

                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<bool>.Fail("Server xətası baş verdi", ex.Message));
            }
        }

    }
}
