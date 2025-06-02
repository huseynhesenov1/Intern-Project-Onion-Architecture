using Project.Application.Abstractions.Repositories.Campaign;
using Project.Application.Abstractions.Repositories.Product;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.ProductDTOs;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Persistance.Implementations.Services.InternalServices
{
    public class ProductService : IProductService
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly ICampaignReadRepository _campaignReadRepository;
        public ProductService(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, ICampaignReadRepository campaignReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _campaignReadRepository = campaignReadRepository;
        }

        public async Task<int> CreateAsync(ProductCreateDTO productCreateDTO)
        {
            Product product = new Product()
            {
                Price = productCreateDTO.Price,
                Title = productCreateDTO.Title,
            };
            product.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _productWriteRepository.CreateAsync(product);
            await _productWriteRepository.SaveChangeAsync();
            return product.Id;
        }

        public async Task<int> UpdateAsync(int Id, ProductUpdateDTO productUpdateDTO)
        {
            Product product = await _productReadRepository.GetByIdAsync(Id, false);
            if (product == null)
            {
                throw new Exception("Invalid ID");
            }
            Product newProduct = new Product()
            {
                Price = productUpdateDTO.Price,
                Title = productUpdateDTO.Title,
            };
            newProduct.Id = Id;
            newProduct.UpdatedAt = DateTime.UtcNow.AddHours(4);
            _productWriteRepository.Update(newProduct);
            await _productWriteRepository.SaveChangeAsync();
            return newProduct.Id;
        }


        public async Task<ICollection<ProductReadDTO>> GetAllAsync()
        {
            var products = await _productReadRepository.GetAllAsync(false);
            var campaigns = await _campaignReadRepository.GetAllAsync(false);
            var currentTime = DateTime.UtcNow.AddHours(4);


            var activeCampaign = campaigns.FirstOrDefault(c =>
                c.IsActive &&
                c.StartDate <= currentTime &&
                c.EndDate >= currentTime);

            List<ProductReadDTO> productReadDTOs = products.Select(p =>
            {
                decimal newPrice = p.Price;
                string campaignName = string.Empty;
                int campaignId = -1;
                //int distirctId = -1; // 

                if (activeCampaign != null)
                {
                    newPrice = p.Price * (1 - activeCampaign.DiscountPercent / 100);
                    campaignName = activeCampaign.Name;
                    campaignId = activeCampaign.Id;
                }

                return new ProductReadDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = newPrice,
                    OldPrice = p.Price,
                    CampaignName = campaignName,
                    CampaignId = campaignId,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                };
            }).ToList();

            return productReadDTOs;
        }

        public async Task<PagedResult<Product>> GetPaginatedAsync(PaginationParams @params)
        {
            var allCategories = await _productReadRepository.GetAllAsync(false);

            var filtered = allCategories
                .Skip((@params.PageNumber - 1) * @params.PageSize)
                .Take(@params.PageSize)
                .ToList();
            int totalCount = allCategories.Count;
            return new PagedResult<Product>(filtered, totalCount, @params.PageNumber, @params.PageSize);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var worker = await _productReadRepository.GetByIdAsync(id, true);
                if (worker == null || worker.IsDeleted)
                {
                    return ApiResponse<bool>.Fail("Product not found", "Invalid Product ID");
                }

                _productWriteRepository.SoftDelete(worker);
                await _productWriteRepository.SaveChangeAsync();

                return ApiResponse<bool>.Success(true, "Product deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message, "Error deleting Product");
            }
        }

        public async Task<ApiResponse<ProductReadDTO>> GetByIdAsync(int id)
        {
            try
            {
                var product = await _productReadRepository.GetByIdAsync(id, false);
                if (product == null || product.IsDeleted)
                {
                    return ApiResponse<ProductReadDTO>.Fail("Product not found", "Invalid Product ID");
                }
                ProductReadDTO productReadDTO = new ProductReadDTO()
                {
                    Id = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    OldPrice = product.Price,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt,

                };

                return ApiResponse<ProductReadDTO>.Success(productReadDTO, "Product retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductReadDTO>.Fail(ex.Message, "Error retrieving Product");
            }
        }

        public async Task<ICollection<ProductReadDTO>> SearchProductsAsync(string title)
        {
            var query = await _productReadRepository.GetAllAsync(false);

            query = query
            .Where(p =>
            (string.IsNullOrWhiteSpace(title) ||
             p.Title.Contains(title, StringComparison.OrdinalIgnoreCase))).ToList();
            var workerDTOs = query.Select(p => new ProductReadDTO
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                OldPrice = p.Price,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,

            }).ToList();
            return workerDTOs;
        }


    }
}
