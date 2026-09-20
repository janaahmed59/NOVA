using Microsoft.Extensions.DependencyInjection;
using MediatR;
namespace NOVA.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            return services;
        }
    }
}
