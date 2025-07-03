using Project.Application.Abstractions.Repositories.Campaign;
using Project.Application.Abstractions.Repositories.Product;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.Abstractions.UnitOfWork;
using Project.Application.DTOs.ProductDTOs;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;

namespace Project.Persistance.Implementations.Services.InternalServices
{
    public class ProductService : IProductService
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly ICampaignReadRepository _campaignReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, ICampaignReadRepository campaignReadRepository, IUnitOfWork unitOfWork)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _campaignReadRepository = campaignReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateAsync(CreateProductInput productCreateDTO)
        {
            var product = new Product
            {
                Title = productCreateDTO.Title,
                Price = productCreateDTO.Price
            };
            var result = await _productWriteRepository.CreateAsync(product);
            if (result == null)
                throw new Exception("Product could not be created.");

            await _unitOfWork.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> UpdateAsync(int id, UpdateProductInput productUpdateDTO)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product == null || product.IsDeleted)
                throw new Exception("Product not found");
            product.Title = productUpdateDTO.Title;
            product.Price = productUpdateDTO.Price;
            product.UpdatedAt = DateTime.UtcNow.AddHours(4);
            _productWriteRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();
            return product.Id;
        }

        public async Task<ICollection<CreateProductOutput>> GetAllAsync()
        {
            var products = await _productReadRepository.GetAllAsync( false);
            var campaigns = await _campaignReadRepository.GetAllAsync( false);
            var currentTime = DateTime.UtcNow.AddHours(4);


            var activeCampaign = campaigns.FirstOrDefault(c =>
                c.IsActive &&
                c.StartDate <= currentTime &&
                c.EndDate >= currentTime);

            List<CreateProductOutput> productReadDTOs = products.Select(p =>
            {
                decimal newPrice = p.Price;
                string campaignName = string.Empty;
                int campaignId = -1;

                if (activeCampaign != null)
                {
                    newPrice = p.Price * (1 - activeCampaign.DiscountPercent / 100);
                    campaignName = activeCampaign.Name;
                    campaignId = activeCampaign.Id;
                }

                return new CreateProductOutput
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

        //public async Task<PagedResult<Product>> GetPaginatedAsync(PaginationParams paginationParams)
        //{
        //    var all = await _productReadRepository.GetAllAsync(false);
        //    var filtered = all.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToList();
        //    return new PagedResult<Product>(filtered, all.Count, paginationParams.PageNumber, paginationParams.PageSize);
        //}


        public async Task<PagedResult<CreateProductOutput>> GetPaginatedAsync(PaginationParams paginationParams)
        {
            var all = await _productReadRepository.GetAllAsync(false);

            var filtered = all
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            var mapped = filtered.Select(p => new CreateProductOutput
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                OldPrice = p.Campaign != null
                    ? Math.Round(p.Price / (1 - (p.Campaign.DiscountPercent / 100m)), 2)
                    : p.Price, // Kampaniya yoxdursa, OldPrice = Price
                CampaignId = p.CampaignId ?? 0,
                CampaignName = p.Campaign != null ? p.Campaign.Name : "Yoxdur",
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();

            return new PagedResult<CreateProductOutput>(
                items: mapped,
                totalCount: all.Count,
                pageNumber: paginationParams.PageNumber,
                pageSize: paginationParams.PageSize
            );
        }

        public async Task<CreateProductOutput> GetByIdAsync(int id)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product == null || product.IsDeleted)
                throw new Exception("Product not found");

            return new CreateProductOutput
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                OldPrice = product.Price,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product == null || product.IsDeleted)
                throw new Exception("Product not found");
            _productWriteRepository.SoftDelete(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<CreateProductOutput>> SearchProductsAsync(string title)
        {
            var products = await _productReadRepository.GetAllAsync(false);

            return products
                .Where(p => string.IsNullOrWhiteSpace(title) || p.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .Select(p => new CreateProductOutput
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    OldPrice = p.Price,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToList();
        }
    }
}
