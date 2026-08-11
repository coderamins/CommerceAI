using CommerceAI.Application.Interfaces;
using CommerceAI.Infrastructure.Persistence;
using CommerceAI.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommerceAI.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString(
                    configuration.GetConnectionString("DefaultConnection") ?? ""));
        });

        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
