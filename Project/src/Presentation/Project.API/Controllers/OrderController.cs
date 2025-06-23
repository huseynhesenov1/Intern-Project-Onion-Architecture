using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.OrderDTOs;
using Project.Application.Models;
using Project.Domain.Entities.Commons;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
       

        [HttpPost]
        public async Task<ApiResponse<CreateOrderResponse>> Create([FromBody] CreateOrderInput orderCreateDTO)
        {
            try
            {
                var result = await _orderService.CreateAsync(orderCreateDTO);
                return ApiResponse<CreateOrderResponse>.Success(result, "Sifariş uğurla yaradıldı");
            }
            catch (Exception ex)
            {
                return ApiResponse<CreateOrderResponse>.Fail("Sifariş yaradılarkən xəta baş verdi", ex.Message);
            }
        }




        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _orderService.GetAllAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] PaginationParams @params)
        {
            var result = await _orderService.GetPaginatedAsync(@params);
            return Ok(result);
        }
    }
}
