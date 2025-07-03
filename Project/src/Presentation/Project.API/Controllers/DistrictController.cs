using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.API.Models;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.DistrictDTOs;
using Project.Domain.Entities;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService _districtService;

        public DistrictController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _districtService.GetAllAsync();
                return Ok(ApiResponse<ICollection<CreateDistrictOutput>>.Success(result, "Bütün rayonlar alındı"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ICollection<CreateDistrictOutput>>.Fail("Server xətası", ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _districtService.GetByIdAsync(id);
                return Ok(ApiResponse<District>.Success(result, "Rayon tapıldı"));
            }
            catch (Exception ex)
            {
                return StatusCode(404, ApiResponse<CreateDistrictOutput>.Fail("Rayon tapılmadı", ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateDistrictInput input)
        {
            try
            {
                var result = await _districtService.CreateAsync(input);
                return StatusCode(201, ApiResponse<District>.Success(result, "Rayon yaradıldı"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<int>.Fail("Yaratma zamanı xəta baş verdi", ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateDistrictInput input)
        {
            try
            {
                var result = await _districtService.UpdateAsync(id, input);
                return Ok(ApiResponse<District>.Success(result, "Rayon yeniləndi"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Fail("Yeniləmə zamanı xəta baş verdi", ex.Message));
            }
        }

        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                var result = await _districtService.SoftDeleteAsync(id);
                return Ok(ApiResponse<District>.Success(result, "Rayon soft silindi"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Fail("Silinmə zamanı xəta baş verdi", ex.Message));
            }
        }
    }
}


