using Microsoft.Extensions.DependencyInjection;
using Project.Application.Abstractions.Services.ExternalServices;
using Project.Application.Abstractions.Services.InternalServices;

namespace Project.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplication(this IServiceCollection services)
        {
            //services.AddScoped<IWorkerService, WorkerService>();
            //services.AddScoped<IProductService, ProductService>();
            //services.AddScoped<IDistrictService, DistrictService>();
            //services.AddScoped<ICampaignService, CampaignService>();
            //services.AddScoped<IOrderService, OrderService>();
            //services.AddScoped<IProductDistrictService, ProductDistrictService>();
            //services.AddScoped<IJwtService, JwtService>();
        }
    }
}
