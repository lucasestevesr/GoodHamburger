using GoodHamburger.Application.Auth.Interfaces;
using GoodHamburger.Application.Auth.Services;
using GoodHamburger.Application.Orders.Interfaces;
using GoodHamburger.Application.Orders.Services;
using GoodHamburger.Application.Products.Interfaces;
using GoodHamburger.Application.Products.Services;
using GoodHamburger.Application.Users.Interfaces;
using GoodHamburger.Application.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.CrossCutting.IoC
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
