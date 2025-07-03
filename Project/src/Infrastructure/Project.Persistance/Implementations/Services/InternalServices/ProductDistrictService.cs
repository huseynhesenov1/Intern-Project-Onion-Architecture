using Project.Application.Abstractions.Repositories.ProductDistrictPrice;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.ProductDistrictPriceDTOs;
using Project.Domain.Entities;

namespace Project.Persistance.Implementations.Services.InternalServices
{
    public class ProductDistrictService : IProductDistrictService
    {
        private readonly IProductDistrictPriceReadRepository _productDistrictPriceReadRepository;
        private readonly IProductDistrictPriceWriteRepository _productDistrictPriceWriteRepository;

        public ProductDistrictService(IProductDistrictPriceWriteRepository productDistrictPriceWriteRepository, IProductDistrictPriceReadRepository productDistrictPriceReadRepository)
        {
            _productDistrictPriceWriteRepository = productDistrictPriceWriteRepository;
            _productDistrictPriceReadRepository = productDistrictPriceReadRepository;
        }

        public async Task<ProductDistrictPrice> CreateAsync(CreateProductDistrictPriceInput dto)
        {
            ProductDistrictPrice productDistrictPrice = new ProductDistrictPrice()
            {
                Price = dto.Price,
                ProductId = dto.ProductId,
                DistrictId = dto.DistrictId,
            };
            var res = await _productDistrictPriceWriteRepository.CreateAsync(productDistrictPrice);
            await _productDistrictPriceWriteRepository.SaveChangeAsync();
            return res;
        }

        public async Task<ICollection<ProductDistrictPrice>> GetAllAsync()
        {
            ICollection<ProductDistrictPrice> result = await _productDistrictPriceReadRepository.GetAllAsync(false);
            return result;
        }
    }
}
