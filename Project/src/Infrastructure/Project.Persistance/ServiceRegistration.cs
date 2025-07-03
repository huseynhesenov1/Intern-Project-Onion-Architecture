using Microsoft.Extensions.DependencyInjection;
using Project.Application.Abstractions.Repositories.Campaign;
using Project.Application.Abstractions.Repositories.District;
using Project.Application.Abstractions.Repositories.Order;
using Project.Application.Abstractions.Repositories.Product;
using Project.Application.Abstractions.Repositories.ProductDistrictPrice;
using Project.Application.Abstractions.Repositories.Worker;
using Project.Application.Abstractions.Services.ExternalServices;
using Project.Application.Abstractions.Services.InternalServices;
using Project.Application.Abstractions.UnitOfWork;
using Project.Persistance.Implementations.Repositories;
using Project.Persistance.Implementations.Repositories.Campaign;
using Project.Persistance.Implementations.Repositories.District;
using Project.Persistance.Implementations.Repositories.Order;
using Project.Persistance.Implementations.Repositories.Product;
using Project.Persistance.Implementations.Repositories.ProductDistrictPrice;
using Project.Persistance.Implementations.Repositories.Worker;
using Project.Persistance.Implementations.Services.ExternalServices;
using Project.Persistance.Implementations.Services.InternalServices;
using Project.Persistance.Implementations.UnitOfWork;

namespace Project.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistance(this IServiceCollection services)
        {
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductDistrictService, ProductDistrictService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IWorkerReadRepository, WorkerReadRepository>();
            services.AddScoped<IWorkerWriteRepository, WorkerWriteRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IDistrictReadRepository, DistrictReadRepository>();
            services.AddScoped<IDistrictWriteRepository, DistrictWriteRepository>();
            services.AddScoped<ICampaignWriteRepository, CampaignWriteRepository>();
            services.AddScoped<ICampaignReadRepository, CampaignReadRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IProductDistrictPriceWriteRepository, ProductDistrictPriceWriteRepository>();
            services.AddScoped<IProductDistrictPriceReadRepository, ProductDistrictPriceReadRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}