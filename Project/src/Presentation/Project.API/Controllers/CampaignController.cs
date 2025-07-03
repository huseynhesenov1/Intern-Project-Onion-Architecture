using Microsoft.AspNetCore.Mvc;
using Project.API.Models;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.Campaign;
using Project.Application.Exceptions;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignController : ControllerBase
{
    private readonly ICampaignService _campaignService;

    public CampaignController(ICampaignService campaignService)
    {
        _campaignService = campaignService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _campaignService.GetAllAsync();
            return Ok(ApiResponse<ICollection<CampaignOutput>>.Success(result, "Uğurla alındı"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ICollection<CampaignOutput>>.Fail("Server xətası baş verdi", ex.Message));
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _campaignService.GetByIdAsync(id);
            return Ok(ApiResponse<CampaignOutput>.Success(result, "Uğurla tapıldı"));
        }
        catch (CampaignNotFoundException ex)
        {
            return NotFound(ApiResponse<CampaignOutput>.Fail("Kampaniya tapılmadı", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CampaignOutput>.Fail("Server xətası baş verdi", ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCampaignInput input)
    {
        try
        {
            var result = await _campaignService.CreateAsync(input);
            return Ok(ApiResponse<int>.Success(result, "Kampaniya yaradıldı"));
        }
        catch (CampaignConflictException ex)
        {
            return BadRequest(ApiResponse<int>.Fail("Tarix konflikti", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<int>.Fail("Server xətası baş verdi", ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCampaignInput input)
    {
        try
        {
            var result = await _campaignService.UpdateAsync(id, input);
            return Ok(ApiResponse<int>.Success(result, "Kampaniya yeniləndi"));
        }
        catch (CampaignNotFoundException ex)
        {
            return NotFound(ApiResponse<int>.Fail("Tapılmadı", ex.Message));
        }
        catch (CampaignConflictException ex)
        {
            return BadRequest(ApiResponse<int>.Fail("Tarix konflikti", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<int>.Fail("Server xətası baş verdi", ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _campaignService.DeleteAsync(id);
            return Ok(ApiResponse<bool>.Success(true, "Kampaniya silindi"));
        }
        catch (CampaignNotFoundException ex)
        {
            return NotFound(ApiResponse<bool>.Fail("Tapılmadı", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.Fail("Server xətası baş verdi", ex.Message));
        }
    }

    [HttpPut("{id}/Enable")]
    public async Task<IActionResult> Enable(int id)
    {
        try
        {
            await _campaignService.EnableAsync(id);
            return Ok(ApiResponse<bool>.Success(true, "Aktiv edildi"));
        }
        catch (CampaignNotFoundException ex)
        {
            return NotFound(ApiResponse<bool>.Fail("Tapılmadı", ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<bool>.Fail("Əməliyyat xətası", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.Fail("Server xətası baş verdi", ex.Message));
        }
    }

    [HttpPut("{id}/Disable")]
    public async Task<IActionResult> Disable(int id)
    {
        try
        {
            await _campaignService.DisableAsync(id);
            return Ok(ApiResponse<bool>.Success(true, "Deaktiv edildi"));
        }
        catch (CampaignNotFoundException ex)
        {
            return NotFound(ApiResponse<bool>.Fail("Tapılmadı", ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<bool>.Fail("Əməliyyat xətası", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.Fail("Server xətası baş verdi", ex.Message));
        }
    }

    [HttpGet("Paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] PaginationParams query)
    {
        try
        {
            var result = await _campaignService.GetPaginatedAsync(query);
            return Ok(ApiResponse<PagedResult<CampaignOutput>>.Success(result, "Paged nəticələr"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<PagedResult<Campaign>>.Fail("Server xətası baş verdi", ex.Message));
        }
    }
}





