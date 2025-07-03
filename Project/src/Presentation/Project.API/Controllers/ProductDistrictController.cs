using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.API.Models;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.ProductDistrictPriceDTOs;
using Project.Domain.Entities;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDistrictController : ControllerBase
    {
        private readonly IProductDistrictService _productDistrictService;
        public ProductDistrictController(IProductDistrictService productDistrictService)
        {
            _productDistrictService = productDistrictService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDistrictPriceInput input)
        {
            try
            {
                var result = await _productDistrictService.CreateAsync(input);
                return Ok(ApiResponse<ProductDistrictPrice>.Success(result, "Məhsul-rayon qiyməti yaradıldı"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<int>.Fail("Yaradılarkən xəta baş verdi", ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _productDistrictService.GetAllAsync();
                return Ok(ApiResponse<ICollection<ProductDistrictPrice>>.Success(result, "Bütün məhsul-rayon qiymətləri alındı"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ICollection<ProductDistrictPrice>>.Fail("Xəta baş verdi", ex.Message));
            }
        }
    }
}
