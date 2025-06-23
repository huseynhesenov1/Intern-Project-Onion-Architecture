using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.ProductDTOs;
using Project.Application.Models;
using Project.Domain.Entities.Commons;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ApiResponse<int>> Create([FromBody] CreateProductInput productCreateDTO)
        {
            try
            {
                var id = await _productService.CreateAsync(productCreateDTO);
                return ApiResponse<int>.Success(id, "Product created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message, "Error creating product");
            }
        }

        [HttpPut("{Id}")]
        public async Task<ApiResponse<int>> Update(int Id, [FromBody] UpdateProductInput productUpdateDTO)
        {
            try
            {
                var id = await _productService.UpdateAsync(Id, productUpdateDTO);
                return ApiResponse<int>.Success(id, "Product updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message, "Error updating product");
            }
        }

        [HttpGet]
        public async Task<ApiResponse<ICollection<CreateProductOutput>>> GetAll()
        {
            try
            {
                var data = await _productService.GetAllAsync();
                return ApiResponse<ICollection<CreateProductOutput>>.Success(data);
            }
            catch (Exception ex)
            {
                return ApiResponse<ICollection<CreateProductOutput>>.Fail(ex.Message, "Error retrieving products");
            }
        }

        [HttpGet("Paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] PaginationParams @params)
        {
            try
            {
                var result = await _productService.GetPaginatedAsync(@params);
                return Ok(ApiResponse<PagedResult<Project.Domain.Entities.Product>>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Fail(ex.Message, "Error retrieving paginated products"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<CreateProductOutput>> GetById(int id)
        {
            try
            {
                var result = await _productService.GetByIdAsync(id);
                return ApiResponse<CreateProductOutput>.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<CreateProductOutput>.Fail(ex.Message, "Error retrieving product");
            }
        }

        [HttpGet("Search")]
        public async Task<ApiResponse<ICollection<CreateProductOutput>>> GetSearch([FromQuery] string title)
        {
            try
            {
                var result = await _productService.SearchProductsAsync(title);
                return ApiResponse<ICollection<CreateProductOutput>>.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<ICollection<CreateProductOutput>>.Fail(ex.Message, "Error searching products");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<bool>> Delete(int id)
        {
            try
            {
                var result = await _productService.DeleteAsync(id);
                return ApiResponse<bool>.Success(result, "Product deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message, "Error deleting product");
            }
        }
    }
}
