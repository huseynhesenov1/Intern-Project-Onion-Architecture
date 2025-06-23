using Microsoft.AspNetCore.Mvc;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.Campaign;
using Project.Application.Exceptions;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;
//using Project.Domain.Exceptions;

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
        var result = await _campaignService.GetAllAsync();
        return Ok(ApiResponse<ICollection<CampaignOutput>>.Success(result, "Uğurla alındı"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _campaignService.GetByIdAsync(id);
            return Ok(ApiResponse<CampaignOutput>.Success(result, "Uğurlu"));
        }
        catch (CampaignNotFoundException ex)
        {
            return NotFound(ApiResponse<CampaignOutput>.Fail("Tapılmadı", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<string>.Fail("Xəta baş verdi", ex.Message));
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
            return StatusCode(500, ApiResponse<string>.Fail("Xəta baş verdi", ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCampaignInput input)
    {
        try
        {
            var result = await _campaignService.UpdateAsync(id, input);
            return Ok(ApiResponse<int>.Success(result, "Yeniləndi"));
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
            return StatusCode(500, ApiResponse<string>.Fail("Xəta baş verdi", ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _campaignService.DeleteAsync(id);
            return Ok(ApiResponse<bool>.Success(true, "Silindi"));
        }
        catch (CampaignNotFoundException ex)
        {
            return NotFound(ApiResponse<bool>.Fail("Tapılmadı", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<string>.Fail("Server xətası baş verdi", ex.Message));
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
            return StatusCode(500, ApiResponse<string>.Fail("Server xətası baş verdi", ex.Message));
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
            return StatusCode(500, ApiResponse<string>.Fail("Server xətası baş verdi", ex.Message));
        }
    }

    [HttpGet("Paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] PaginationParams @params)
    {
        var result = await _campaignService.GetPaginatedAsync(@params);
        return Ok(ApiResponse<PagedResult<Campaign>>.Success(result, "Paged nəticələr"));
    }
}

