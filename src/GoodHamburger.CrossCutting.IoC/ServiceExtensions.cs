using GoodHamburger.Application.Orders.Interfaces;
using GoodHamburger.Application.Orders.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.CrossCutting.IoC
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
