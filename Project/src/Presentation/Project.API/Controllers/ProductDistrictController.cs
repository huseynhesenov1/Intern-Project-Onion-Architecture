using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.ProductDistrictPriceDTOs;

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
        public async Task<IActionResult> Create([FromBody] ProductDistrictPriceCreateDTO productDistrictPriceCreateDTO)
        {
            try
            {
                var res = await _productDistrictService.CreateAsync(productDistrictPriceCreateDTO);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _productDistrictService.GetAllAsync();
            return Ok(res);
        }
    }
}
