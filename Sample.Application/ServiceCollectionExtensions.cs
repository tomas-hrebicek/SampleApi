using Microsoft.Extensions.DependencyInjection;
using Sample.Application.Interfaces;
using Sample.Application.Services;

namespace Sample.Application
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services for application layer
        /// </summary>
        /// <param name="services">service collection</param>
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            AddServices(services);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IProductsService, ProductsService>();
        }
    }
}