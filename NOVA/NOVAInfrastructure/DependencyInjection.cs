using Microsoft.Extensions.DependencyInjection;
using NOVA.Application.Common.Interfaces;
using NOVAInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
namespace NOVAInfrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IApplicationDbContext>(
            sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}