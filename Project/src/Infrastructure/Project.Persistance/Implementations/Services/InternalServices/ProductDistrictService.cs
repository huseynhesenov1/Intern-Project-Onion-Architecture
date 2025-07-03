using Project.Application.Abstractions.Repositories.ProductDistrictPrice;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.Abstractions.UnitOfWork;
using Project.Application.DTOs.ProductDistrictPriceDTOs;
using Project.Domain.Entities;

namespace Project.Persistance.Implementations.Services.InternalServices
{
    public class ProductDistrictService : IProductDistrictService
    {
        private readonly IProductDistrictPriceReadRepository _productDistrictPriceReadRepository;
        private readonly IProductDistrictPriceWriteRepository _productDistrictPriceWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductDistrictService(IProductDistrictPriceWriteRepository productDistrictPriceWriteRepository, IProductDistrictPriceReadRepository productDistrictPriceReadRepository, IUnitOfWork unitOfWork)
        {
            _productDistrictPriceWriteRepository = productDistrictPriceWriteRepository;
            _productDistrictPriceReadRepository = productDistrictPriceReadRepository;
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.SaveChangesAsync();
            return res;
        }

        public async Task<ICollection<ProductDistrictPrice>> GetAllAsync()
        {
            ICollection<ProductDistrictPrice> result = await _productDistrictPriceReadRepository.GetAllAsync(false);
            return result;
        }
    }
}
