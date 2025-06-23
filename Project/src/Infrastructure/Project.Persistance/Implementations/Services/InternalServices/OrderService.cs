using Microsoft.AspNetCore.Http;
using Project.Application.Abstractions.Repositories.Campaign;
using Project.Application.Abstractions.Repositories.Order;
using Project.Application.Abstractions.Repositories.Product;
using Project.Application.Abstractions.Repositories.Worker;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.DTOs.OrderDTOs;
using Project.Application.Models;
using Project.Domain.Entities;
using Project.Domain.Entities.Commons;
using System.Security.Claims;

namespace Project.Persistance.Implementations.Services.InternalServices
{
    public class OrderService : IOrderService
    {
        private readonly IWorkerReadRepository _workerReadRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly ICampaignReadRepository _campaignReadRepository;
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IHttpContextAccessor httpContextAccessor, IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, ICampaignReadRepository campaignReadRepository, IProductReadRepository productReadRepository, IWorkerReadRepository workerReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _campaignReadRepository = campaignReadRepository;
            _productReadRepository = productReadRepository;
            _workerReadRepository = workerReadRepository;
        }
        public async Task<CreateOrderResponse> CreateAsync(CreateOrderInput dto)
        {
            var workerIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (workerIdClaim == null)
                throw new Exception("Unauthorized");

            var workerId = int.Parse(workerIdClaim.Value);
            var worker = await _workerReadRepository.GetByIdAsync(workerId, true, "District");
            if (worker == null)
                throw new Exception("Worker not found");

            var product = await _productReadRepository.GetByIdAsync(dto.ProductId, true, "ProductDistrictPrices");
            if (product == null)
                throw new Exception("Product not found");

            decimal finalPrice = product.Price;

            var campaigns = await _campaignReadRepository.GetAllAsync(false, false);
            var activeCampaign = campaigns.FirstOrDefault(c =>
                c.IsActive &&
                DateTime.UtcNow >= c.StartDate &&
                DateTime.UtcNow <= c.EndDate
            );

            if (activeCampaign != null)
            {
                finalPrice *= (1 - activeCampaign.DiscountPercent / 100m);
            }

            var districtPriceEntity = product.ProductDistrictPrices
                .FirstOrDefault(p => p.DistrictId == worker.DistrictId);

            if (districtPriceEntity != null)
            {
                finalPrice -= districtPriceEntity.Price;
            }

            decimal totalPrice = finalPrice * dto.ProductCount;

            var order = new Order
            {
                ProductId = dto.ProductId,
                ProductCount = dto.ProductCount,
                WorkerId = workerId,
                TotalPrice = totalPrice,
                CreatedAt = DateTime.UtcNow.AddHours(4)
            };

            await _orderWriteRepository.CreateAsync(order);
            await _orderWriteRepository.SaveChangeAsync();

            return new CreateOrderResponse
            {
                TotalPrice = totalPrice
            };
        }
       


        public async Task<PagedResult<CreateOrderOutput>> GetPaginatedAsync(PaginationParams @params)
        {
            var orders = await _orderReadRepository.GetAllAsync(false, false, o=>o.Product);
            var campaigns = await _campaignReadRepository.GetAllAsync(false, false);

            var orderDTOs = orders.Select(o =>
            {
                var activeCampaign = campaigns.FirstOrDefault(c =>
                    c.IsActive &&
                    c.StartDate <= o.CreatedAt &&
                    c.EndDate >= o.CreatedAt);

                return new CreateOrderOutput
                {
                    Id = o.Id,
                    ProductId = o.ProductId,
                    ProductCount = o.ProductCount,
                    ProductTitle = o.Product.Title,
                    ProductPrice = o.Product.Price,
                    CampaignId = activeCampaign?.Id ?? 0,
                    CampaignName = activeCampaign?.Name ?? string.Empty,
                    TotalPrice = o.TotalPrice,
                    CreatedAt = o.CreatedAt
                };
            }).ToList();

            int totalCount = orderDTOs.Count;

            var paginatedDTOs = orderDTOs
                .Skip((@params.PageNumber - 1) * @params.PageSize)
                .Take(@params.PageSize)
                .ToList();

            return new PagedResult<CreateOrderOutput>(paginatedDTOs, totalCount, @params.PageNumber, @params.PageSize);
        }


        public async Task<ICollection<CreateOrderOutput>> GetAllAsync()
        {
            var orders = await _orderReadRepository.GetAllAsync(false, false, o=>o.Product);
            var campaigns = await _campaignReadRepository.GetAllAsync(false , false);
            
            return orders.Select(o =>
            {
                var activeCampaign = campaigns.FirstOrDefault(c =>
                   
                    c.StartDate <= o.CreatedAt &&
                    c.EndDate >= o.CreatedAt);

                return new CreateOrderOutput
                {
                    Id = o.Id,
                    ProductId = o.ProductId,
                    ProductCount = o.ProductCount,
                    ProductTitle = o.Product.Title,
                    ProductPrice = o.Product.Price,
                    CampaignId = activeCampaign?.Id ?? 0,
                    CampaignName = activeCampaign?.Name ?? string.Empty,
                    TotalPrice = o.TotalPrice,
                    CreatedAt = o.CreatedAt
                };
            }).ToList();
        }
    }
}
